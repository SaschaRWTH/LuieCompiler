using LUIECompiler.CodeGeneration.Exceptions;


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

        /// <summary>
        /// Gets the size in the context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="InternalException"></exception>
        public static int GetSize(this LuieParser.DeclarationContext context)
        {
            if (!int.TryParse(context?.size?.Text, out int result))
            {
                throw new InternalException()
                {
                    Reason = "Could not parse the index. This should not occure!"
                };
            }
            return result;
        }

        /// <summary>
        /// Tries to get the size of the register.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="size"> Size of the register. </param>
        /// <returns> True, if a valid <paramref name="size"/> was given, overwise false. </returns>
        public static bool TryGetSize(this LuieParser.DeclarationContext context, out int size)
        {
            size = 0;

            if (!int.TryParse(context?.size?.Text, out int result))
            {
                return false;
            }
            size = result;
            return true;
        }
    }
}