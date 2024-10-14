using LUIECompiler.CodeGeneration.Definitions;

namespace LUIECompiler.Optimization.Graphs
{
    public class GraphRegisterAccess : GraphQubit
    {
        public int Index { get; }

        /// <summary>
        /// Currently prevent any register access from being removed.
        /// Needs extra logic for translations
        /// </summary>
        public override bool CanBeRemoved => false;

        public GraphRegisterAccess(CircuitGraph graph, UniqueIdentifier identifier, int index) : base(graph, identifier)
        {
            Index = index;
        }

        public override string ToString()
        {
            return $"GraphQubit = {{ Identifier = {Identifier}, Index = {Index} }}";
        }
    }
}