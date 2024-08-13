using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Optimization.Graphs.Interfaces;
using LUIECompiler.Optimization.Graphs.Nodes;

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

        /// <summary>
        /// Extends the vertex to the given <paramref name="node"/>.
        /// </summary>
        /// <param name="vertex"></param>
        public void ExtendTo(INode node)
        {
            End = node;
            if (node is OutputNode outNode)
            {
                outNode.InputVertex = this;
            }

            if(node is GateNode gateNode)
            {
                IVertex old = gateNode.GetInVertex(Qubit) ?? throw new InternalException()
                {
                    Reason = $"The input vertex is missing for qubit {Qubit}."
                };
                
                gateNode.InputVertices.Remove(old);
                gateNode.InputVertices.Add(this);
            }
        }
    }
}