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
            throw new NotImplementedException();
        }
    }
}