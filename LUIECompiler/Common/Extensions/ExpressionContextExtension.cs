using System.Numerics;
using LUIECompiler.CodeGeneration.Expressions;

namespace LUIECompiler.Common.Extensions
{
    public static class ExpressionContextExtension
    {
        public static Expression<T> GetExpression<T>(this LuieParser.ExpressionContext context) where T : INumber<T>
        {
            if(context.op is null)
            {
                return context.term().GetExpression<T>();
            }

            throw new NotImplementedException();
        }
    }
}