
using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.CodeGeneration.Statements
{
    /// <summary>
    /// Represents a skip statement.
    /// </summary>
    public class SkipStatement : Statement
    {
        public override QASMProgram ToQASM(CodeGenerationContext context)
        {
            // Nothing executed in QASM -> Return nothing for the skip statement
            return new();
        }
    }

}