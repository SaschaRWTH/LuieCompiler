using LUIECompiler.Optimization.Graphs.Interfaces;

namespace LUIECompiler.Optimization.Graphs
{
    public class Vertex : IVertex
    {
        public IGraph Graph { get; }

        public INode Start { get; protected set; }

        public INode End { get; protected set; }

        /// <summary>
        /// Creates a new vertex.
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public Vertex(IGraph graph, INode start, INode end)
        {
            Start = start;
            End = end;
            Graph = graph;

            Start.AddOutput(this);
            End.AddInput(this);
        }


        public override string ToString()
        {
            return $"Vertex = {{ Start = {Start}, End = {End} }}";
        }
    }
}