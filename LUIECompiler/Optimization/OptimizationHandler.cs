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
            
            graph.ApplyOptimizationRules(NullGateRule.NullGateRules, NullGateRule.MaxRuleLength);
            
            return graph.ToQASM();
        }




    }
}