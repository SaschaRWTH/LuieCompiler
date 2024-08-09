using System.Numerics;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Extensions;

namespace LUIECompiler.CodeGeneration.Expressions
{
    /// <summary>
    /// Represents a power function expression.
    /// </summary>
    /// <typeparam name="T">Type of the result.</typeparam>
    public class PowerFunctionExpression<T> : FunctionExpression<T> where T : INumber<T>
    {
        /// <summary>
        /// List of parameters of the function.
        /// </summary>
        public List<Expression<T>> Parameters { get; init; }

        /// <summary>
        /// Creates a power function expression from the given <paramref name="parameters"/>.
        /// </summary>
        /// <param name="parameters"></param>
        public PowerFunctionExpression(List<Expression<T>> parameters)
        {
            Parameters = parameters;
        }

        /// <summary>
        /// Creates a power function expression from the given <paramref name="context"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public PowerFunctionExpression(LuieParser.FunctionParameterContext context)
        {
            // TODO: Add error handling for wrong parameters
            Parameters = context.expression()?.Select(expression => expression.GetExpression<T>()).ToList() ?? throw new NotImplementedException();
        }

        public override T Evaluate(CodeGenerationContext context)
        {
            if (Parameters.Count != 2)
            {
                throw new CodeGenerationException()
                {
                    Error = new InvalidFunctionArguments(new ErrorContext(), "Power", 2, Parameters.Count),
                };
            }

            Expression<T> baseExp = Parameters[0];
            Expression<T> exponentExp = Parameters[1];

            double baseValue = double.CreateChecked(baseExp.Evaluate(context));
            double exponent = double.CreateChecked(exponentExp.Evaluate(context));

            return T.CreateChecked(Math.Pow(baseValue, exponent));
        }

        public override List<string> UndefinedIdentifiers(SymbolTable table)
        {
            List<string> result = new List<string>();
            foreach (var parameter in Parameters)
            {
                result.AddRange(parameter.UndefinedIdentifiers(table));
            }
            return result;
        }

        public override string ToString()
        {
            return $"power({Parameters[0]}, {Parameters[1]})";
        }
    }
}