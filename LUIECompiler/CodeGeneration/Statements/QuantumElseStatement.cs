
using LUIECompiler.Common;
using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.CodeGeneration.Statements
{
    public class QuantumElseStatement : QuantumIfStatement
    {

        public QuantumElseStatement() {}

        public QuantumElseStatement(RegisterInfo register, CodeBlock block) : base(register, block)
        {
        }

        public override QASMCode ToQASM()
        {
            return Block.ToQASM().AddControl(identifier: GetIdentifier(Guard), negated: true);
        }
    }

}