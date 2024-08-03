using System.Numerics;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Expressions
{
    public enum FunctionType
    {
        SizeOf,
    }

    public static class FunctionTypeExtension
    {
        public static Func<List<string>, CodeGenerationContext, T> GetFunction<T>(this FunctionType function) where T : INumber<T>
        {
            return function switch
            {
                FunctionType.SizeOf => SizeOf<T>,
                _ => throw new NotImplementedException(),
            };
        }

        private static T SizeOf<T>(List<string> parameters, CodeGenerationContext context) where T : INumber<T>
        {
            if (parameters.Count != 1)
            {
                throw new CodeGenerationException()
                {
                    Error = new InvalidFunctionArguments(new ErrorContext(), FunctionType.SizeOf, 1, parameters.Count),
                };
            }

            string identifier = parameters[0];

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

    public class FunctionExpression<T> : Expression<T> where T : INumber<T>
    {
        public required FunctionType Type { get; init; }

        public required List<string> Parameter { get; init; }

        public override T Evaluate(CodeGenerationContext context)
        {
            return Type.GetFunction<T>()(Parameter, context);
        }

        public override List<string> UndefinedIdentifiers(SymbolTable table)
        {
            return Parameter.Where(p => !table.IsDefined(p)).ToList();
        }
    }
}