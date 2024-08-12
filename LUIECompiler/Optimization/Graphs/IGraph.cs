using LUIECompiler.Optimization.Graphs.Nodes;

namespace LUIECompiler.Optimization.Graphs
{
    public interface IGraph
    {
        public List<INode> Nodes { get; }
        public List<IVertex> Vertices { get; }

        public void AddNode(INode node);

        public void AddVertex(IVertex vertex);

        public void ReplacePath(Path old, Path replacement);
    }
}