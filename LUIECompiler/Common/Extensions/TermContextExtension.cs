using System.Numerics;
using LUIECompiler.CodeGeneration.Expressions;

namespace LUIECompiler.Common.Extensions
{
    public static class TermContextExtension
    {
        public static Expression<T> GetExpression<T>(this LuieParser.TermContext context) where T : INumber<T>
        {
            if(context.op is null)
            {
                return context.factor().GetExpression<T>();
            }

            throw new NotImplementedException();
        }
    }
}