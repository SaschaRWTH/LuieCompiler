using System.Numerics;

namespace LUIECompiler.CodeGeneration.Expressions
{
    /// <summary>
    /// Represents an abstract function expression.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class FunctionExpression<T> : Expression<T> where T : INumber<T>
    {
        /// <summary>
        /// Creates a function expression from the given <paramref name="context"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static FunctionExpression<T> FromContext(LuieParser.FunctionContext context)
        {
            string function = context.func.Text;
            return function switch
            {
                "sizeof" => new SizeOfFunctionExpression<T>(context.param),
                "power" => new PowerFunctionExpression<T>(context.param),
                _ => throw new NotImplementedException(),
            };
        } 
    }
}