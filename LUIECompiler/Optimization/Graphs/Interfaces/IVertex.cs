namespace LUIECompiler.Optimization.Graphs.Interfaces
{
    public interface IVertex
    {
        public INode Start { get; }
        public INode End { get; }

        public IGraph Graph { get; }
    }
}