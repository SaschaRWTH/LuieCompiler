using System.Collections;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Optimization.Sequences;

namespace LUIECompiler.Optimization
{
    /// <summary>
    /// Represents a set of independent sequences.
    /// </summary>
    public class IndependentSequenceSet : IEnumerable<CodeSequence>
    {
        private readonly List<CodeSequence> _sequences;

        /// <summary>
        /// Creates an empty instance of <see cref="IndependentSequenceSet"/>.
        /// </summary>
        public IndependentSequenceSet()
        {
            _sequences = new List<CodeSequence>();
        }

        /// <summary>
        /// Creates an instance of <see cref="IndependentSequenceSet"/> with a given <paramref name="code"/>.
        /// </summary>
        /// <param name="code"></param>
        public IndependentSequenceSet(IEnumerable<Code> code)
        {
            _sequences = new List<CodeSequence>();
            foreach (Code c in code)
            {
                AddCode(c);
            }
        }

        /// <summary>
        /// Adds a <paramref name="code"/> to the set of independent sequences.
        /// </summary>
        /// <param name="code"></param>
        public void AddCode(Code code)
        {
            CodeSequence sequence = GetDependetSequence(code);
            sequence.AddCode(code);
        }

        /// <summary>
        /// Gets the dependent sequence of a given <paramref name="code"/>.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public CodeSequence GetDependetSequence(Code code)
        {
            if(_sequences.Count == 0 || code is not GateApplicationCode gateCode)
            {
                return CreateNewSequence();
            }

            IEnumerable<CodeSequence> dependetSequences = _sequences.Where(s => !s.IndependentOf(gateCode));
            if(dependetSequences.Count() != 1)
            {
                return CreateNewSequence();
            }

            return dependetSequences.First();
        }

        /// <summary>
        /// Creates a new sequence and adds it to the set of independent sequences.
        /// </summary>
        /// <returns></returns>
        public CodeSequence CreateNewSequence()
        {
            CodeSequence sequence = new CodeSequence();
            _sequences.Add(sequence);
            return sequence;
        }

        public IEnumerator<CodeSequence> GetEnumerator()
        {
            return _sequences.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _sequences.GetEnumerator();
        }
    }
}