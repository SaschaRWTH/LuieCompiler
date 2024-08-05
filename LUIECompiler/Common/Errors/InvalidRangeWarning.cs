using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.Common.Errors
{
    /// <summary>
    /// Represents a warning that the range is invalid.
    /// </summary>
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
        
        /// <summary>
        /// Creates a new invalid range warning.
        /// </summary>
        /// <param name="context">Context where the range was created.</param>
        /// <param name="start">Start value of the range.</param>
        /// <param name="end">End value of the range.</param>
        public InvalidRangeWarning(ErrorContext context, int start, int end)
        {
            Type = ErrorType.Warning;
            ErrorContext = context;
            Start = start;
            End = end;
            Description = $"The range from {start} to {end} is invalid and has a size of {end-start}.";
        }
    }

}