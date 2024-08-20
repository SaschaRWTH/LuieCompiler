namespace LUIECompiler.Optimization.Graphs
{
    public class GraphGuard
    {
        public bool Negated { get; } 

        public GraphQubit Qubit { get; }

        public GraphGuard(GraphQubit qubit, bool negated)
        {
            Qubit = qubit;
            Negated = negated;
        } 
    }
}