using LUIECompiler.CodeGeneration.Definitions;

namespace LUIECompiler.Optimization.Graphs
{
    public class GraphRegisterAccess : GraphQubit
    {
        public int Index { get; }

        public GraphRegisterAccess(CircuitGraph graph, UniqueIdentifier identifier, int index) : base(graph, identifier)
        {
            Index = index;
        }
    }
}