using System.Numerics;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Extensions;

namespace LUIECompiler.CodeGeneration.Expressions
{
    /// <summary>
    /// Represents the minimum function expression
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MinimumFunctionExpression<T> : FunctionExpression<T> where T : INumber<T>
    {
        /// <summary>
        /// The parameters of the function.
        /// </summary>
        public List<Expression<T>> Parameters { get; set; }

        /// <summary>
        /// Creates a new instance of the minimum function expression.
        /// </summary>
        /// <param name="parameters">Parameters of the function</param>
        public MinimumFunctionExpression(List<Expression<T>> parameters)
        {
            Parameters = parameters;
        }

        /// <summary>
        /// Creates a new instance of the minimum function expression.
        /// </summary>
        /// <param name="context">Context of the function</param>
        /// <exception cref="NotImplementedException"></exception>
        public MinimumFunctionExpression(LuieParser.FunctionParameterContext context)
        {
            // TODO: Add error handling for wrong parameters
            Parameters = context.expression()?.Select(expression => expression.GetExpression<T>()).ToList() ?? throw new NotImplementedException();
        }

        public override T Evaluate(CodeGenerationContext context)
        {
            if (Parameters.Count == 0)
            {
                throw new CodeGenerationException()
                {
                    // TODO: Add error context
                    Error = new InvalidFunctionArguments(new(), "min", 1, 0),
                };
            }

            T result = Parameters[0].Evaluate(context);

            foreach (Expression<T> parameter in Parameters)
            {
                T value = parameter.Evaluate(context);
                if (value < result)
                {
                    result = value;
                }
            }

            return result;
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
    }
}