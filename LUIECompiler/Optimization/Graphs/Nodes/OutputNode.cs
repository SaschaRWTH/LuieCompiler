
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Optimization.Graphs.Interfaces;

namespace LUIECompiler.Optimization.Graphs.Nodes
{
    public class OutputNode : Node
    {
        public override List<IVertex> InputVertices => InputVertex == null ? [] : [InputVertex];
        public override List<IVertex> OutputVertices => [];

        public IVertex? InputVertex;

        public OutputNode(IGraph graph) : base(graph)
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

        public IVertex GetVertex()
        {
            if(InputVertex == null)
            {
                throw new InternalException()
                {
                    Reason = "Output node must have exactly one output vertex",
                };
            }

            return InputVertex;
        }
        
        public override string ToString()
        {
            return "OutputNode";
        }
    }
}