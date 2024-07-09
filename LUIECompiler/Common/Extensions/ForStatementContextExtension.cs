using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.Common.Extensions
{
    public static class ForStatementContextExtension
    {
        public static LoopIterator GetIterator(this LuieParser.ForstatementContext context)
        {
            string identifier = context.IDENTIFIER().GetText();

            Range range = context.range().GetRange();

            return new(identifier, range.Start.Value, range.End.Value);
        }
    }
}