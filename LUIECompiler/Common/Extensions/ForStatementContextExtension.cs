using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.Common.Extensions
{
    public static class ForStatementContextExtension
    {
        public static LoopIterator GetIterator(this LuieParser.ForstatementContext context)
        {
            string identifier = context.IDENTIFIER().GetText();

            LuieParser.RangeContext range = context.range();
            if (!int.TryParse(range.start.Text, out int start) || !int.TryParse(range.end.Text, out int end))
            {
                throw new InternalException()
                {
                    Reason = "Failed to parse the range of the for statement.",
                };
            }

            // TODO: Add invalid range check

            return new(identifier, start, end);
        }
    }
}