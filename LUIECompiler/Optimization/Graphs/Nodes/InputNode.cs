
using LUIECompiler.CodeGeneration.Exceptions;

namespace LUIECompiler.Optimization.Graphs.Nodes
{
    public class InputNode : Node
    {
        public override List<IVertex> InputVertices => [];
        public override List<IVertex> OutputVertices { get; } = [];
        
        public InputNode(IGraph graph) : base(graph)
        {

        }


        public IVertex GetVertex()
        {
            if(OutputVertices.Count != 1)
            {
                throw new InternalException()
                {
                    Reason = "Output node must have exactly one output vertex",
                };
            }

            return OutputVertices[0];
        }
    }
}