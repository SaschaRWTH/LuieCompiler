using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Optimization.Rules;
using LUIECompiler.Optimization.Sequences;

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
            IEnumerable<Code> gateApplications = HoistDefinitions();
            IndependentSequenceSet independentSequences = new(gateApplications);
            ApplyRules(NullGateRule.NullGateRules, NullGateRule.MaxRuleLength, independentSequences);

            Console.WriteLine("Optimized code:");
            foreach (Code c in Program.Code)
            {
                Console.WriteLine(c);
            }

            return Program;
        }

        /// <summary>
        /// Hoists all definitions to the top of the program so that they can be ignored while optimizing.
        /// </summary>
        /// <returns>An enumerable of all gate applications.</returns>
        public IEnumerable<Code> HoistDefinitions()
        {
            IEnumerable<Code> definitions = Program.Code.Where(c => c is DefinitionCode);
            IEnumerable<Code> gateApplications = Program.Code.Where(c => c is GateApplicationCode);

            Program.Code = definitions.Concat(gateApplications).ToList();
            return gateApplications;
        }

        /// <summary>
        /// Finds all independent sequences in the program.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InternalException"></exception>
        public List<QASMSubsequence> FindIndependentSequences()
        {
            List<QASMSubsequence> independentSequences = [];
            // Can only work correctly if the definitions were hoisted previously.
            int startIndex = Program.Code.FindIndex(c => c is GateApplicationCode);
            QASMSubsequence independent = new(startIndex, Program);
            independentSequences.Add(independent);
            for (int i = startIndex; i < Program.Code.Count; i++)
            {
                if (Program.Code[i] is not GateApplicationCode gateCode)
                {
                    throw new InternalException()
                    {
                        Reason = "Code is not a gate application code. Any definition should have been hoisted previously."
                    };
                }

                if (!independent.IndependentOf(gateCode))
                {
                    independent.Code.Add(gateCode);
                }
                else
                {
                    independent = new(i, Program);
                    independentSequences.Add(independent);
                    independent.Code.Add(gateCode);
                }
            }

            return independentSequences;
        }

        /// <summary>
        /// Applies the given <paramref name="rules"/> to the given <paramref name="independentSequences"/>.
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="maxDepth"></param>
        /// <param name="independentSequences"></param>
        public void ApplyRules(IEnumerable<IRule> rules, int maxDepth, IndependentSequenceSet independentSequences)
        {
            List<CodeSequence> code = [];
            foreach (CodeSequence codeSeq in independentSequences)
            {
                Console.WriteLine("Optimizing code sequence:");
                Console.WriteLine(codeSeq);
                CodeSequence optimized = ApplyRules(rules, maxDepth, codeSeq);
                code.Add(optimized);
            }

            List<Code> optimizedCode = Program.Code.Where(c => c is DefinitionCode).ToList();
            optimizedCode.AddRange(code.SelectMany(c => c.Code));

            Program.Code = optimizedCode;
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
                if (tryAgain)
                {
                    result = optimized;
                }
            }

            return result;
        }
    }
}