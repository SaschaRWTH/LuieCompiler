using LUIECompiler.CodeGeneration.Expressions;
using LUIECompiler.Common.Errors;

namespace LUIECompiler.Common.Symbols
{
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

        public LoopIterator(string identifier, Expression<int> start, Expression<int> end, ErrorContext errorContext) : base(identifier, errorContext)
        {
            Start = start;
            End = end;
        }
    }
}