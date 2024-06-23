
using LUIECompiler.CodeGeneration.Code;
namespace LUIECompiler.CodeGeneration.Statements
{
    public class SkipStatement : AbstractStatement
    {
        public override QASMCode ToQASM()
        {
            // Nothing executed in QASM -> Return nothing for the skip statement
            return new();
        }
    }

}