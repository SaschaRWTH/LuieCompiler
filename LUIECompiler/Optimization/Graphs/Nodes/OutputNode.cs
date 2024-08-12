
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Optimization.Graphs.Interfaces;

namespace LUIECompiler.Optimization.Graphs.Nodes
{
    /// <summary>
    /// Represents an output node in a quantum circuit graph.
    /// </summary>
    public class OutputNode : Node
    {
        public override List<IVertex> InputVertices => InputVertex == null ? [] : [InputVertex];
        public override List<IVertex> OutputVertices => [];

        /// <summary>
        /// The input vertex of the node.
        /// </summary>
        public IVertex? InputVertex;

        /// <summary>
        /// Creates a new output node.
        /// </summary>
        /// <param name="graph"></param>
        public OutputNode(CircuitGraph graph) : base(graph)
        {
        }
        public override void AddOutput(IVertex vertex)
        {
            throw new InternalException()
            {
                Reason = "Output node cannot have output vertex",
            };
        }

        public override void AddInput(IVertex vertex)
        {
            InputVertex = vertex;
        }
        
        public override string ToString()
        {
            return "OutputNode";
        }
    }
}