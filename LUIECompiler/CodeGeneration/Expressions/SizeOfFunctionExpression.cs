using System.Numerics;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Extensions;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Expressions
{
    public class SizeOfFunctionExpression<T> : FunctionExpression<T> where T : INumber<T>
    {
        public List<string> Parameter { get; }

        public SizeOfFunctionExpression(List<string> parameter)
        {
            Parameter = parameter;
        }

        public SizeOfFunctionExpression(LuieParser.FunctionParameterContext context)
        {
            // TODO: Adjust error handling!
            Parameter = context.IDENTIFIER()?.Select(obj => obj.GetText())?.ToList() ?? throw new NotImplementedException();
        }

        public override List<string> UndefinedIdentifiers(SymbolTable table)
        {
            return Parameter.Where(obj => obj is string).Select(obj => (string)obj).Where(p => !table.IsDefined(p)).ToList();
        }

        public override T Evaluate(CodeGenerationContext context)
        {
            if (Parameter.Count != 1)
            {
                throw new CodeGenerationException()
                {
                    Error = new InvalidFunctionArguments(new ErrorContext(), "SizeOf", 1, Parameter.Count),
                };
            }

            string identifier = Parameter[0] as string ?? throw new InternalException()
            {
                Reason = "The parameter is not a string.",
            };

            Symbol parameter = context.CurrentBlock.GetSymbol(identifier, context);
            if (parameter is not Register register)
            {
                throw new CodeGenerationException()
                {
                    Error = new TypeError(new ErrorContext(), parameter.Identifier, typeof(Register), parameter.GetType()),
                };
            }

            // This is a bad solution, but I can not think of a better one right now.
            List<Constant<int>> intConstants = new();
            foreach (var constant in context.IntegerConstants)
            {
                intConstants.Add(new Constant<int>(constant.Identifier, int.CreateChecked(constant.Value), constant.ErrorContext));
            }

            return T.CreateChecked(register.Size.Evaluate(context));
        }
    }
}