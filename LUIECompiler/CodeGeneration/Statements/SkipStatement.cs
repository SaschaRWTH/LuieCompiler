
using LUIECompiler.CodeGeneration.Codes;
namespace LUIECompiler.CodeGeneration.Statements
{
    public class SkipStatement : Statement
    {
        public override QASMCode ToQASM()
        {
            // Nothing executed in QASM -> Return nothing for the skip statement
            return new();
        }
    }

}