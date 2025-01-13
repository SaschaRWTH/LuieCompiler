using System.Numerics;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Extensions;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Expressions
{
    /// <summary>
    /// Represents a sizeof function expression.
    /// </summary>
    /// <typeparam name="T">Type of the result.</typeparam>
    public class SizeOfFunctionExpression<T> : FunctionExpression<T> where T : INumber<T>
    {
        /// <summary>
        /// List of parameters of the function.
        /// </summary>
        public List<string> Parameter { get; }


        /// <summary>
        /// Creates a sizeof function expression from the given <paramref name="context"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public SizeOfFunctionExpression(LuieParser.FunctionParameterContext context)
        {
            Parameter = context.IDENTIFIER()?.Select(obj => obj.GetText())?.ToList() ?? throw new NotImplementedException();
            ArgumentErrorContext = new ErrorContext(context);
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
                    Error = new InvalidFunctionArguments(ArgumentErrorContext, "SizeOf", 1, Parameter.Count),
                };
            }

            string identifier = Parameter[0] as string ?? throw new InternalException()
            {
                Reason = "The parameter is not a string.",
            };

            Symbol parameter = context.CurrentBlock.GetSymbol(identifier, context);
            if (parameter is not Register register)
            {
                Compiler.LogError($"SizeOf parameter '{identifier}' is not a register.");
                throw new CodeGenerationException()
                {
                    Error = new TypeError(new ErrorContext(), parameter.Identifier, typeof(Register), parameter.GetType()),
                };
            }

            return T.CreateChecked(register.Size.Evaluate(context));
        }

        public override string ToString()
        {
            return $"sizeof({Parameter[0]})";
        }
    }
}