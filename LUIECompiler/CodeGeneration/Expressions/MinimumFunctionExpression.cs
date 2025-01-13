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
        public List<Expression<T>> Arguments { get; set; }

        /// <summary>
        /// Creates a new instance of the minimum function expression.
        /// </summary>
        /// <param name="context">Context of the function</param>
        /// <exception cref="NotImplementedException"></exception>
        public MinimumFunctionExpression(LuieParser.FunctionParameterContext context)
        {
            if(TryIdentifierToIdentifierExpression(context, out List<Expression<T>> expressions))
            {
                Arguments = expressions;
                return;
            }
            
            Arguments = context.expression()?.Select(expression => expression.GetExpression<T>()).ToList() ?? throw new NotImplementedException();
            ArgumentErrorContext = new ErrorContext(context);
        }

        public override T Evaluate(CodeGenerationContext context)
        {
            if (Arguments.Count == 0)
            {
                throw new CodeGenerationException()
                {
                    Error = new InvalidFunctionArguments(ArgumentErrorContext, "min", 1, 0),
                };
            }

            T result = Arguments[0].Evaluate(context);

            foreach (Expression<T> parameter in Arguments)
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
            foreach (var parameter in Arguments)
            {
                result.AddRange(parameter.UndefinedIdentifiers(table));
            }
            return result;
        }
    }
}