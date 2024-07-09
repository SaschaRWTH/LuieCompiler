using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.Common.Errors
{
    public class InvalidRangeWarning : CompilationError
    {
        public int Start { get; init; }
        public int End { get; init; }
        public InvalidRangeWarning(int line, int start, int end)
        {
            Type = ErrorType.Warning;
            Line = line;
            Start = start;
            End = end;
            Description = $"The range from {start} to {end} is invalid and has a size of {end-start}.";
        }
    }

}