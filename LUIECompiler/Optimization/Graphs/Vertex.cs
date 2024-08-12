using LUIECompiler.Optimization.Graphs.Interfaces;

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

            Console.WriteLine($"Vertex created: {this}");

            Start.AddOutput(this);
            End.AddInput(this);
        }


        public override string ToString()
        {
            return $"Vertex = {{ Start = {Start}, End = {End} }}";
        }
    }
}