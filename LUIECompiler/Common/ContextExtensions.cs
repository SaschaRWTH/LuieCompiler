using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.Common
{
    public static class ContextExtensions
    {
        public static string GetIdentifier(this LuieParser.RegisterContext context)
        {
            return context.IDENTIFIER().GetText();
        }

        public static bool HasIndex(this LuieParser.RegisterContext context)
        {
            return context.index != null;
        }

        public static int GetIndex(this LuieParser.RegisterContext context)
        {
            if (!int.TryParse(context?.index?.Text, out int result))
            {
                throw new InternalException()
                {
                    Reason = "Could not parse the index. This should not occure!"
                };
            }
            return result;
        }

        public static bool TryGetIndex(this LuieParser.RegisterContext context, out int index)
        {
            index = 0;

            if (!int.TryParse(context?.index?.Text, out int result))
            {
                return false;
            }
            index = result;
            return true;
        }

        public static bool HasSize(this LuieParser.DeclarationContext context)
        {
            return context.size != null;
        }

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