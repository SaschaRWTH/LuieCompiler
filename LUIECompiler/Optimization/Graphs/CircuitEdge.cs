using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Optimization.Graphs.Interfaces;
using LUIECompiler.Optimization.Graphs.Nodes;

namespace LUIECompiler.Optimization.Graphs
{
    /// <summary>
    /// Represents a edge in a circuit graph.
    /// </summary>
    public class CircuitEdge : Edge
    {
        /// <summary>
        /// The qubit the edge is associated with.
        /// </summary>
        public GraphQubit Qubit { get; }

        /// <summary>
        /// Creates a new circuit edge.
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="qubit"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public CircuitEdge(IGraph graph, GraphQubit qubit, INode start, INode end) : base(graph, start, end)
        {
            Qubit = qubit;
        }

        /// <summary>
        /// Extends the edge to the given <paramref name="node"/>.
        /// </summary>
        /// <param name="edge"></param>
        public void ExtendTo(INode node)
        {
            End = node;
            if (node is OutputNode outNode)
            {
                outNode.InputEdge = this;
            }

            if(node is GateNode gateNode)
            {
                IEdge old = gateNode.GetInEdge(Qubit) ?? throw new InternalException()
                {
                    Reason = $"The input edge is missing for qubit {Qubit}."
                };
                
                gateNode.InputEdges.Remove(old);
                gateNode.InputEdges.Add(this);
            }
        }
    }
}