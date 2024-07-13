using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.CodeGeneration.Expressions;


namespace LUIECompiler.Common.Extensions
{
    public static class DeclarationContextExtension
    {


        /// <summary>
        /// Checks whether a size is given in the context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool HasSize(this LuieParser.DeclarationContext context)
        {
            return context.size != null;
        }

        public static Expression<int> GetSize(this LuieParser.DeclarationContext context)
        {
            return context.size.GetExpression<int>();
        }

        /// <summary>
        /// Tries to get the size of the register.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="size"> Size of the register. </param>
        /// <returns> True, if a valid <paramref name="size"/> was given, overwise false. </returns>
        public static bool TryGetSize(this LuieParser.DeclarationContext context, out Expression<int> size)
        {
            if (context.HasSize())
            {
                size = context.GetSize();
                return true;
            }
            else
            {
                size = new ConstantExpression<int>()
                {
                    Value = 0,
                };
                return false;
            }
        }
    }
}