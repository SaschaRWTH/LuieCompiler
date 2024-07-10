
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common.Symbols;
namespace LUIECompiler.CodeGeneration.Statements
{
    public class SkipStatement : Statement
    {
        public override QASMProgram ToQASM(CodeGenerationContext context)
        {
            // Nothing executed in QASM -> Return nothing for the skip statement
            return new();
        }
    }

}