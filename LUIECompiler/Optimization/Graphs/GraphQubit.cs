using LUIECompiler.Optimization.Graphs.Interfaces;
using LUIECompiler.Optimization.Graphs.Nodes;

namespace LUIECompiler.Optimization.Graphs
{
    public class GraphQubit
    {
        public IGraph Graph { get; }

        public InputNode Start { get; }

        public OutputNode End { get; }

        public string Identifier { get; }

        public GraphQubit(IGraph graph, string identifier)
        {
            Graph = graph;
            Identifier = identifier;

            Start = new(graph);
            End = new(graph);
            graph.AddNode(Start);
            graph.AddNode(End);

            IVertex vertex = new Vertex(graph, Start, End);
            graph.AddVertex(vertex);
        }

        public void AddGateNode(GateNode node)
        {
            IVertex last = End.GetVertex();

            IVertex input = new Vertex(Graph, last.Start, node);
            IVertex output = new Vertex(Graph, node, last.End);

            Graph.ReplacePath(new([last]), new([input, output]));
        }
    }
}