
namespace LUIECompiler.CodeGeneration.Statements
{
    public abstract class SkipStatement : AbstractStatement
    {
        public override QASMCode ToQASM()
        {
            // Nothing executed in QASM -> Return nothing for the skip statement
            return new();
        }
    }

}