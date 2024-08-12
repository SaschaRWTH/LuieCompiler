
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Optimization.Graphs.Interfaces;

namespace LUIECompiler.Optimization.Graphs.Nodes
{
    /// <summary>
    /// Reprents an input node in a quantum circuit graph.
    /// </summary>
    public class InputNode : Node
    {
        public override List<IVertex> InputVertices => [];
        public override List<IVertex> OutputVertices => OutputVertex == null ? [] : [OutputVertex];
        
        /// <summary>
        /// The output vertex of the node.
        /// </summary>
        public IVertex? OutputVertex;

        /// <summary>
        /// Creates a new input node.
        /// </summary>
        /// <param name="graph"></param>
        public InputNode(CircuitGraph graph) : base(graph)
        {

        }

        public override void AddInput(IVertex vertex)
        {
            throw new InternalException()
            {
                Reason = "Input node cannot have input vertex",
            };
        }

        public override void AddOutput(IVertex vertex)
        {
            OutputVertex = vertex;
        }

        public override string ToString()
        {
            return "InputNode";
        }
    }
}