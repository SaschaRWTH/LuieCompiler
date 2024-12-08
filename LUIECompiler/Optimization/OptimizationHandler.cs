using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Optimization.Graphs;

namespace LUIECompiler.Optimization
{
    public class OptimizationHandler
    {
        public QASMProgram Program { get; private set; }

        public OptimizationHandler(QASMProgram program)
        {
            Program = program.ShallowCopy();
        }

        public QASMProgram OptimizeProgram(OptimizationType optimizationType)
        {
            var rules = optimizationType.GetRules();

            if(rules.Count == 0)
            {
                return Program;
            }


            CircuitGraph graph = new(Program);
            bool changed = true;
            while (changed)
            {
                changed = false;
                changed |= graph.ApplyOptimizationRules(rules, rules.Max(rule => rule.MaxRuleDepth));
            }
            Compiler.LogInfo($"No more optimizations possible.");

            graph.RemoveUnusedQubits();

            return graph.ToQASM();
        }




    }
}