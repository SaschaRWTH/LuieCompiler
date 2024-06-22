
using LUIECompiler.Common;

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
            throw new NotImplementedException();
        }
    }

}