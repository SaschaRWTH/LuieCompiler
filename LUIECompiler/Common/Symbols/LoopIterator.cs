namespace LUIECompiler.Common.Symbols
{
    public class LoopIterator : Symbol
    {
        public int Start { get; init; }
        public int End { get; init; }

        public LoopIterator(string identifier, int start, int end) : base(identifier)
        {
            Start = start;
            End = end;
        }
    }
}