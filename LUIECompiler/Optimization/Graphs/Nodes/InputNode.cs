
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Optimization.Graphs.Interfaces;

namespace LUIECompiler.Optimization.Graphs.Nodes
{
    /// <summary>
    /// Reprents an input node in a quantum circuit graph.
    /// </summary>
    public class InputNode : CircuitNode
    {
        public override List<IEdge> InputEdges => [];
        public override List<IEdge> OutputEdges => OutputEdge == null ? [] : [OutputEdge];
        
        /// <summary>
        /// The output edge of the node.
        /// </summary>
        public IEdge? OutputEdge;

        /// <summary>
        /// Creates a new input node.
        /// </summary>
        /// <param name="graph"></param>
        public InputNode(CircuitGraph graph) : base(graph)
        {

        }

        public override void AddInput(IEdge edge)
        {
            throw new InternalException()
            {
                Reason = "Input node cannot have input edge",
            };
        }

        public override void AddOutput(IEdge edge)
        {
            OutputEdge = edge;
        }

        public override string ToString()
        {
            return "InputNode";
        }
    }
}