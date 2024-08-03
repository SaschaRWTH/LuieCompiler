using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.CodeGeneration.Expressions;
namespace LUIECompiler.Common.Extensions
{
    public static class FunctionParameterContextExtension
    {
        /// <summary>
        /// Gets the parameters of the function.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        /// <exception cref="CodeGenerationException"></exception>
        public static List<object> GetParameter(this LuieParser.FunctionParameterContext context)
        {
            var identifiers = context.IDENTIFIER();
            if(identifiers is not null)
            {
                return identifiers.Select(identifier => identifier.GetText()).ToList<object>();
            }
            
            var expressions = context.expression();
            if(expressions is not null)
            {
                return expressions.Select(expression => expression.GetExpression<double>()).ToList<object>();
            }

            throw new InternalException()
            {
                Reason = "The function parameters are neither identifiers nor expressions.",
            };
        }
    }
}