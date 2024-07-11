
using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.CodeGeneration.Statements
{
    public class QuantumElseStatement : QuantumIfStatement
    {
        public override QASMProgram ToQASM(CodeGenerationContext context)
        {
            QubitCode qubit = GetGuardCode(context);
            return Block.ToQASM(context).AddGuard(qubit: qubit, negated: true);
        }
    }

}