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
        public static Func<List<string>, List<Constant<T>>, CodeBlock, T> GetFunction<T>(this FunctionType function) where T : INumber<T>
        {
            return function switch
            {
                FunctionType.SizeOf => SizeOf,
                _ => throw new NotImplementedException(),
            };
        }

        private static T SizeOf<T>(List<string> parameters, List<Constant<T>> constants, CodeBlock codeBlock) where T : INumber<T>
        {
            if (parameters.Count != 1)
            {
                throw new CodeGenerationException()
                {
                    Error = new InvalidFunctionArguments(new ErrorContext(), FunctionType.SizeOf, 1, parameters.Count),
                };
            }

            string identifier = parameters[0];

            Symbol parameter = codeBlock.GetSymbol(identifier);
            if (parameter is not Register register)
            {
                throw new CodeGenerationException()
                {
                    Error = new TypeError(new ErrorContext(), parameter.Identifier, typeof(Register), parameter.GetType()),
                };
            }

            // This is a bad solution, but I can not think of a better one right now.
            List<Constant<int>> intConstants = new();
            foreach (var constant in constants)
            {
                intConstants.Add(new Constant<int>(constant.Identifier, int.CreateChecked(constant.Value), constant.ErrorContext));
            }

            return T.CreateChecked(register.Size.Evaluate(intConstants, codeBlock));
        }
    }

    public class FunctionExpression<T> : Expression<T> where T : INumber<T>
    {
        public required FunctionType Type { get; init; }

        public required List<string> Parameter { get; init; }

        public override T Evaluate(List<Constant<T>> constants, CodeBlock codeBlock)
        {
            return Type.GetFunction<T>()(Parameter, constants, codeBlock);
        }

        public override List<string> UndefinedIdentifiers(SymbolTable table)
        {
            return Parameter.Where(p => !table.IsDefined(p)).ToList();
        }
    }
}