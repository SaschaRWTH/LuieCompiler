using LUIECompiler.Optimization.Graphs.Interfaces;

namespace LUIECompiler.Optimization.Graphs
{
    /// <summary>
    /// Represents a vertex in a circuit graph.
    /// </summary>
    public class CircuitVertex : Vertex
    {   
        /// <summary>
        /// The qubit the vertex is associated with.
        /// </summary>
        public GraphQubit Qubit { get; }

        /// <summary>
        /// Creates a new circuit vertex.
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="qubit"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public CircuitVertex(IGraph graph, GraphQubit qubit, INode start, INode end) : base(graph, start, end)
        {
            Qubit = qubit;
        }
    }
}