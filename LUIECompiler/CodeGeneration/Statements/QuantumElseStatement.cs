
using LUIECompiler.Common;
using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.CodeGeneration.Statements
{
    public class QuantumElseStatement : QuantumIfStatement
    {
        public override QASMProgram ToQASM()
        {
            QubitCode qubit = GetGuardCode();
            return Block.ToQASM().AddGuard(qubit: qubit, negated: true);
        }
    }

}