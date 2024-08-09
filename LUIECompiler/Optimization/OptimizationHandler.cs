using System.Security.Cryptography.X509Certificates;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Optimization.Rules;

namespace LUIECompiler.Optimization
{
    public class OptimizationHandler
    {
        public QASMProgram Program { get; set; }

        public OptimizationHandler(QASMProgram program)
        {
            Program = program;
        }

        public void OptimizeSingleQubitNullGates()
        {
            throw new NotImplementedException();
        }

        public void ApplyRules(IEnumerable<IRule> rules, int maxDepth, List<CodeSequence> independentSequences)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Applies the given <paramref name="rules"/> to the given <paramref name="independentSequences"/>.
        /// </summary>
        /// <param name="rules">Rules to apply.</param>
        /// <param name="maxDepth">Maximum depth to apply the rules.</param>
        /// <param name="independentSequences">Independent sequences to apply the rules to.</param>
        /// <returns>Optimized code sequence.</returns>
        public CodeSequence ApplyRules(IEnumerable<IRule> rules, int maxDepth, CodeSequence independentSequences)
        {
            CodeSequence result = independentSequences;

            // This should not loop infinitely, as any applied rule should reduce the length of the code sequence.
            bool tryAgain = true;
            while (tryAgain)
            {
                tryAgain = result.ApplyRules(rules, maxDepth, out CodeSequence optimized);
                result = optimized;
            }

            return result;
        }
    }
}