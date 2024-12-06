
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Optimization.Graphs.Interfaces;

namespace LUIECompiler.Optimization.Graphs.Nodes
{
    /// <summary>
    /// Represents an output node in a quantum circuit graph.
    /// </summary>
    public class OutputNode : CircuitNode
    {
        public override List<IEdge> InputEdges => InputEdge == null ? [] : [InputEdge];
        public override List<IEdge> OutputEdges => [];

        /// <summary>
        /// The input edge of the node.
        /// </summary>
        public IEdge? InputEdge;

        /// <summary>
        /// Creates a new output node.
        /// </summary>
        /// <param name="graph"></param>
        public OutputNode(CircuitGraph graph) : base(graph)
        {
        }
        public override void AddOutput(IEdge edge)
        {
            throw new InternalException()
            {
                Reason = "Output node cannot have output edge",
            };
        }

        public override void AddInput(IEdge edge)
        {
            InputEdge = edge;
        }
        
        public override string ToString()
        {
            return "OutputNode";
        }
    }
}