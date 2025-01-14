using LUIECompiler.CodeGeneration.Expressions;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;


namespace LUIECompiler.Common.Extensions
{
    public static class RegisterDeclarationContextExtension
    {
        /// <summary>
        /// Checks whether a size is given in the context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool HasSize(this LuieParser.RegisterDeclarationContext context)
        {
            return context.size != null;
        }

        /// <summary>
        /// Gets the size of the register definition.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Expression<int> GetSize(this LuieParser.RegisterDeclarationContext context, SymbolTable symbolTable)
        {
            return context.size.GetExpression<int>(symbolTable);
        }

        /// <summary>
        /// Tries to get the size of the register.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="size"> Size of the register. </param>
        /// <returns> True, if a valid <paramref name="size"/> was given, overwise false. </returns>
        public static bool TryGetSize(this LuieParser.RegisterDeclarationContext context, SymbolTable symbolTable, out Expression<int> size)
        {
            if (context.HasSize())
            {
                size = context.GetSize(symbolTable);
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

        /// <summary>
        /// Gets the register from the declaration context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Register GetRegister(this LuieParser.RegisterDeclarationContext context, SymbolTable symbolTable)
        {
            string identifier = context.IDENTIFIER().GetText();
            Register register;

            if (context.TryGetSize(symbolTable, out Expression<int> size))
            {
                register = new Register(identifier, size, new ErrorContext(context));
            }
            else
            {
                register = new Qubit(identifier, new ErrorContext(context));
            }

            return register;
        }
    }
}