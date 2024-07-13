using LUIECompiler.CodeGeneration.Expressions;

namespace LUIECompiler.Common.Extensions
{
    public static class RegisterContextExtension
    {
        /// <summary>
        /// Gets the identifier of the register.
        /// </summary>
        /// <param name="context">Context of the register.</param>
        /// <returns> Identifier of the register. </returns>
        public static string GetIdentifier(this LuieParser.RegisterContext context)
        {
            return context.IDENTIFIER().GetText();
        }

        /// <summary>
        /// Gets the identifiers of all registers.
        /// </summary>
        /// <param name="context"> Contexts of the registers. </param>
        /// <returns> Identifiers of the registers. </returns>
        public static IEnumerable<string> GetIdentifiers(this IEnumerable<LuieParser.RegisterContext> context)
        {
            return context.Select(c => c.GetIdentifier());
        }

        /// <summary>
        /// Tries to get the index that is accessed in the register.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="index"> Index that is accessed. </param>
        /// <returns>True, if a valid <paramref name="index"/> was given, overwise false.</returns>
        public static bool TryGetIndexExpression(this LuieParser.RegisterContext context, out Expression<int> index)
        {
            index = new ConstantExpression<int>()
            {
                Value = 0
            };

            LuieParser.ExpressionContext expression = context.index;
            if(expression == null)
            {
                return false;
            }

            index = expression.GetExpression<int>();
            
            return true;
        }

        /// <summary>
        /// Checks whether a register access is given in the context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool IsRegisterAccess(this LuieParser.RegisterContext context)
        {
            return context.index != null;
        }

    }
}