using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Optimization.Rules;

namespace LUIECompiler.Optimization
{
    /// <summary>
    /// Represents a sequence of codes used for code optimization.
    /// </summary>
    public class CodeSequence
    {
        /// <summary>
        /// List of codes in the sequence.
        /// </summary>
        public List<Code> Code { get; }

        /// <summary>
        /// Indicates whether the sequence is empty.
        /// </summary>
        public bool IsEmpty
        {
            get => Code.Count == 0;
        }

        /// <summary>
        /// Gets the number of codes in the sequence.
        /// </summary>
        public int Count
        {
            get => Code.Count;
        }

        /// <summary>
        /// Indicates whether all code in the sequence are gate applications.
        /// </summary>
        public bool OnlyGatesApplications
        {
            get => Code.All(code => code is GateApplicationCode);
        }

        /// <summary>
        /// Indicates whether all code in the sequence is semantically equal.
        /// </summary>
        public bool SemanticallyEqual
        {
            get
            {
                if (IsEmpty)
                {
                    return true;
                }
                return Code.All(code => code.SemanticallyEqual(Code[0]));
            }
        }

        /// <summary>
        /// Creates an empty code sequence.
        /// </summary>
        public CodeSequence()
        {
            Code = [];
        }


        /// <summary>
        /// Creates a code sequence with the given codes.
        /// </summary>
        /// <param name="codes">Sequence of codes.</param>
        public CodeSequence(List<Code> codes)
        {
            Code = codes;
        }

        /// <summary>
        /// Creates a shallow copy of the code sequence beginning at the given <paramref name="index"/> and containing the given <paramref name="count"/> elements.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public CodeSubsequence GetSubSequence(int index, int count)
        {
            if (index < 0 || index >= Code.Count)
            {
                return new CodeSubsequence(this);
            }

            if (count < 0)
            {
                return new CodeSubsequence(this);
            }

            int cappedCount = Math.Min(count, (Code.Count - 1) - index);
            return new CodeSubsequence(this, cappedCount, Code.GetRange(index, cappedCount));
        }

        /// <summary>
        /// Applies the given <paramref name="rules"/> to the code sequence.
        /// </summary>
        /// <param name="rules">Rules to apply.</param>
        /// <param name="maxDepth">Maximum depth to apply the rules.</param>
        /// <param name="optimized">Resulting optimized code sequence.</param>
        /// <returns>True if a rule was applied, false otherwise.</returns>
        public bool ApplyRules(IEnumerable<IRule> rules, int maxDepth, out CodeSequence optimized)
        {
            optimized = new();
            for(int i = 0; i < Count; i++)
            {   
                // Start at 1, a subsequence of length 0 cannot be optimized.
                for(int j = 1; j < maxDepth && i + j < Count; j++)
                {
                    CodeSubsequence subSequence = GetSubSequence(i, j);
                    if(subSequence.TryApplyRules(rules, out CodeSequence optimizedSeq))
                    {
                        optimized = optimizedSeq;
                        return true;
                    }
                }
            }
            return false;
        }

        public QASMProgram ToQASMProgram()
        {
            return new(Code);
        }
    }
}