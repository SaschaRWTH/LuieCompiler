using LUIECompiler.Optimization.Graphs.Nodes;

namespace LUIECompiler.Optimization.Graphs
{
    public class Vertex : IVertex
    {
        public IGraph Graph { get; }

        public INode Start { get; }

        public INode End { get; }

        public Vertex(IGraph graph, INode start, INode end)
        {
            Start = start;
            End = end;
            Graph = graph;

            Start.AddOutput(this);
            End.AddInput(this);
        }

    }
}