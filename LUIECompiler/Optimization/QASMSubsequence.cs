using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.Optimization
{
    public class QASMSubsequence : QASMProgram, ISubsequence<QASMProgram>
    {
        public int StartIndex { get; }
        public QASMProgram Parent { get; }

        public QASMSubsequence(int startIndex, QASMProgram parent)
        {
            StartIndex = startIndex;
            Parent = parent;
        }

        public QASMProgram Replace(QASMProgram replacement)
        {
            List<Code> codes = [.. Parent.Code];
            
            codes.RemoveRange(StartIndex, replacement.Code.Count);
            codes.InsertRange(StartIndex, replacement.Code);

            return new(codes);
        }

        public CodeSequence ToCodeSequence()
        {
            return new(Code);
        }

        /// <summary>
        /// Checks whether a given <paramref name="code"/> is independent on the subsequence.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool IndependentOf(GateApplicationCode code)
        {
            if(Code.Count == 0)
            {
                return false;
            }

            return Code.All(c => c is GateApplicationCode gateCode && gateCode.AreIndependent(code));
        }
    }
}