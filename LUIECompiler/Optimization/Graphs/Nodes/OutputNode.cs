
using LUIECompiler.CodeGeneration.Exceptions;

namespace LUIECompiler.Optimization.Graphs.Nodes
{
    public class OutputNode : Node
    {
        public override List<IVertex> InputVertices { get; } = [];
        public override List<IVertex> OutputVertices => [];

        public OutputNode(IGraph graph) : base(graph)
        {
        }

        public IVertex GetVertex()
        {
            if(InputVertices.Count != 1)
            {
                throw new InternalException()
                {
                    Reason = "Output node must have exactly one output vertex",
                };
            }

            return InputVertices[0];
        }
    }
}