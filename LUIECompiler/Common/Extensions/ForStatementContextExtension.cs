using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.Common.Extensions
{
    public static class ForStatementContextExtension
    {
        /// <summary>
        /// Gets the iterator of the for statement.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static LoopIterator GetIterator(this LuieParser.ForstatementContext context)
        {
            string identifier = context.IDENTIFIER().GetText();

            return context.range().GetRange(identifier);
        }
    }
}