namespace LUIECompiler.Common.Symbols
{
    public class LoopIterator : Symbol
    {
        /// <summary>
        /// Start of the loop.
        /// </summary>
        public int Start { get; init; }

        /// <summary>
        /// End of the loop.
        /// </summary>
        public int End { get; init; }

        public LoopIterator(string identifier, int start, int end) : base(identifier)
        {
            Start = start;
            End = end;
        }
    }
}