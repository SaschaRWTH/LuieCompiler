using LUIECompiler.Optimization.Graphs.Nodes;

namespace LUIECompiler.Optimization.Graphs
{
    public interface IVertex
    {
        public INode Start { get; }
        public INode End { get; }

        public IGraph Graph { get; }
    }
}