using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Optimization.Rules;

namespace LUIECompiler.Optimization
{
    public class CodeSubsequence : CodeSequence, ISubsequence<CodeSequence>
    {
        public int StartIndex { get; }

        public CodeSequence Parent { get; }

        public CodeSubsequence(CodeSequence parent) : base()
        {
            Parent = parent;
            StartIndex = 0;
        }

        public CodeSubsequence(CodeSequence parent, int startIndex, List<Code> codes) : base(codes)
        {
            Parent = parent;
            StartIndex = startIndex;
        }

        
        /// <summary>
        /// Returns a code sequence where the subsequence in the <see cref="Parent"/> is replace with the given <paramref name="replacement"/>.
        /// </summary>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public CodeSequence Replace(CodeSequence replacement)
        {
            List<Code> codes = [.. Parent.Codes];
            codes.RemoveRange(StartIndex, Count);
            codes.InsertRange(StartIndex, replacement.Codes);
            return new CodeSequence(codes);
        }

        /// <summary>
        /// Tries to apply the given <paramref name="rules"/> to the subsequence and returns the optimized code sequence.
        /// </summary>
        /// <param name="rules">Rules to apply.</param>
        /// <param name="optimized">Resulting optimized code sequence.</param>
        /// <returns>Returns true if the rules were applied, false otherwise.</returns>
        public bool TryApplyRules(IEnumerable<IRule> rules, out CodeSequence optimized)
        {
            optimized = new();
            foreach (var rule in rules)
            {
                if (rule.IsApplicable(this))
                {
                    CodeSequence optimizedSub = rule.Apply(this);
                    optimized = Replace(optimizedSub);
                    return true;
                }
            }
            return false;
        }
    }
}