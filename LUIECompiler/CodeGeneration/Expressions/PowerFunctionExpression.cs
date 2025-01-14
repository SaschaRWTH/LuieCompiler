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
        public List<Expression<T>> Arguments { get; init; }

        /// <summary>
        /// Creates a power function expression from the given <paramref name="context"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public PowerFunctionExpression(LuieParser.FunctionParameterContext context, SymbolTable symbolTable)
        {
            if (TryIdentifierToIdentifierExpression(context, out List<Expression<T>> expressions))
            {
                Arguments = expressions;
                return;
            }

            Arguments = context.expression()?.Select(expression => expression.GetExpression<T>(symbolTable)).ToList() ?? throw new NotImplementedException();
            ArgumentErrorContext = new ErrorContext(context);
        }

        public override T Evaluate(CodeGenerationContext context)
        {
            if (Arguments.Count != 2)
            {
                throw new CodeGenerationException()
                {
                    Error = new InvalidFunctionArguments(ArgumentErrorContext, "Power", 2, Arguments.Count),
                };
            }

            Expression<T> baseExp = Arguments[0];
            Expression<T> exponentExp = Arguments[1];

            double baseValue = double.CreateChecked(baseExp.Evaluate(context));
            double exponent = double.CreateChecked(exponentExp.Evaluate(context));

            return T.CreateChecked(Math.Pow(baseValue, exponent));
        }

        public override List<string> PropagateSymbolInformation(SymbolTable table)
        {
            List<string> result = new List<string>();
            foreach (var parameter in Arguments)
            {
                result.AddRange(parameter.PropagateSymbolInformation(table));
            }
            return result;
        }

        public override string ToString()
        {
            return $"power({Arguments[0]}, {Arguments[1]})";
        }
    }
}