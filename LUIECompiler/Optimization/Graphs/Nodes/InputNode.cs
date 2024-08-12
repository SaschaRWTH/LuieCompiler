
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Optimization.Graphs.Interfaces;

namespace LUIECompiler.Optimization.Graphs.Nodes
{
    public class InputNode : Node
    {
        public override List<IVertex> InputVertices => [];
        public override List<IVertex> OutputVertices => OutputVertex == null ? [] : [OutputVertex];
        
        public IVertex? OutputVertex;

        public InputNode(IGraph graph) : base(graph)
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

        public IVertex GetVertex()
        {
            if(OutputVertex == null)
            {
                throw new InternalException()
                {
                    Reason = "Output node must have exactly one output vertex",
                };
            }

            return OutputVertex;
        }

        public override string ToString()
        {
            return "InputNode";
        }
    }
}