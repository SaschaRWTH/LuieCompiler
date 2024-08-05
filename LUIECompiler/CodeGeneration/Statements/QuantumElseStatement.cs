
using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.CodeGeneration.Statements
{
    /// <summary>
    /// Represents an else statement.
    /// </summary>
    public class QuantumElseStatement : QuantumIfStatement
    {
        public override QASMProgram ToQASM(CodeGenerationContext context)
        {
            QubitCode qubit = GetGuardCode(context);
            return Block.ToQASM(context).AddGuard(qubit: qubit, negated: true);
        }
    }

}