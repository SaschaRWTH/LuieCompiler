using System.Numerics;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.CodeGeneration.Expressions;

namespace LUIECompiler.Common.Extensions
{
    public static class FunctionContextExtension
    {

        /// <summary>
        /// Gets the expression of the <see cref="LuieParser.FunctionContext"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static FunctionExpression<T> GetFunctionExpression<T>(this LuieParser.FunctionContext context) where T : INumber<T>
        {
            return FunctionExpression<T>.FromContext(context);
        }
        
    }
}