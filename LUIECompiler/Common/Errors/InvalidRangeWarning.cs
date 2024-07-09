using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.Common.Errors
{
    public class InvalidRangeWarning : CompilationError
    {
        /// <summary>
        /// The start of the range.
        /// </summary>
        public int Start { get; init; }

        /// <summary>
        /// The end of the range.
        /// </summary>
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