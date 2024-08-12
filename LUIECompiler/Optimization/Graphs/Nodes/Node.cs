
namespace LUIECompiler.Optimization.Graphs.Nodes
{
    public abstract class Node : INode
    {
        public abstract List<IVertex> InputVertices { get; }
        public abstract List<IVertex> OutputVertices { get; }

        public IGraph Graph{ get; }

        public Node(IGraph graph)
        {
            Graph = graph;
        }

        public void AddInput(IVertex vertex)
        {
            InputVertices.Add(vertex);
        }
        
        public void AddOutput(IVertex vertex)
        {
            OutputVertices.Add(vertex);
        }
    }
}