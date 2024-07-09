using LUIECompiler.CodeGeneration.Exceptions;

namespace LUIECompiler.Common.Extensions
{
    public static class RangeContextExtension
    {
        public static Range GetRange(this LuieParser.RangeContext context)
        {
            if (!int.TryParse(context.start.Text, out int start) || !int.TryParse(context.end.Text, out int end))
            {
                throw new InternalException()
                {
                    Reason = "Failed to parse the range of the for statement.",
                };
            }

            return new(start, end);
        }
    }
}