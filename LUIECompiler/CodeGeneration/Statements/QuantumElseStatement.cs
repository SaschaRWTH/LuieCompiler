
using LUIECompiler.Common;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common.Symbols;

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