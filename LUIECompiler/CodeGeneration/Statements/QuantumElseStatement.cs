
using LUIECompiler.Common;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Statements
{
    public class QuantumElseStatement : QuantumIfStatement
    {
        public override QASMProgram ToQASM(List<Constant<int>> constants)
        {
            QubitCode qubit = GetGuardCode(constants);
            return Block.ToQASM(constants).AddGuard(qubit: qubit, negated: true);
        }
    }

}