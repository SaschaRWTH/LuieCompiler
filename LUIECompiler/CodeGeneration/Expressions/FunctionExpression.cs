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
                "min" => new MinimumFunctionExpression<T>(context.param),
                "max" => new MaximumFunctionExpression<T>(context.param),
                _ => throw new NotImplementedException(),
            };
        }

        protected bool TryIdentifierToIdentifierExpression(LuieParser.FunctionParameterContext context, out List<Expression<T>> expressions)
        {
            expressions = new List<Expression<T>>();
            if (context.IDENTIFIER() is not Antlr4.Runtime.Tree.ITerminalNode[] array)
            {
                return false;
            }

            if (array.Length == 0)
            {
                return false;
            }

            foreach (var identifier in array)
            {
                expressions.Add(new IdentifierExpression<T>()
                {
                    Identifier = identifier.GetText()
                });
            }
            return true;
        }
    }
}