using LUIECompiler.CodeGeneration.Expressions;
using LUIECompiler.Common.Errors;

namespace LUIECompiler.Common.Symbols
{
    /// <summary>
    /// Represents a loop iterator.
    /// </summary>
    public class LoopIterator : Symbol
    {
        /// <summary>
        /// Start of the loop.
        /// </summary>
        public Expression<int> Start { get; init; }

        /// <summary>
        /// End of the loop.
        /// </summary>
        public Expression<int> End { get; init; }

        /// <summary>
        /// The current value of the loop iterator when iterating through the loop.
        /// </summary>
        public int CurrentValue { get; set; }

        /// <summary>
        /// Creates a new loop iterator.
        /// </summary>
        /// <param name="identifier">Identifier of the loop iterator.</param>
        /// <param name="start">Start value of the loop.</param>
        /// <param name="end">End value of the loop.</param>
        /// <param name="errorContext">Context of the loop iterator definition.</param>
        public LoopIterator(string identifier, Expression<int> start, Expression<int> end, ErrorContext errorContext) : base(identifier, errorContext)
        {
            Start = start;
            End = end;
        }

        public override string ToString()
        {
            return $"LoopIterator: {{ id={Identifier}, start={Start}, end={End} }}";
        }
    }
}