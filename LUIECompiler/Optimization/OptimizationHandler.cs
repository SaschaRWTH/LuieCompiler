using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Optimization.Graphs;
using LUIECompiler.Optimization.Rules;

namespace LUIECompiler.Optimization
{
    public class OptimizationHandler
    {
        public QASMProgram Program { get; set; }

        public OptimizationHandler(QASMProgram program)
        {
            Program = program.ShallowCopy();
        }

        public QASMProgram OptimizeSingleQubitNullGates()
        {
            CircuitGraph graph = new(Program);

            bool changed = true;
            while (changed)
            {
                changed = false;
                changed |= graph.ApplyOptimizationRules(NullGateRule.NullGateRules, NullGateRule.MaxRuleLength);
            }

            return graph.ToQASM();
        }




    }
}