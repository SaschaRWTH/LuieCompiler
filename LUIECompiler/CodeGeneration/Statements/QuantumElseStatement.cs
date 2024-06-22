
using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration.Statements
{
    public class QuantumElseStatment : QuantumIfStatment
    {

        public QuantumElseStatment() {}
        
        public QuantumElseStatment(RegisterInfo register, CodeBlock block) : base(register, block)
        {
        }

        public override QASMCode ToQASM()
        {
            throw new NotImplementedException();
        }
    }

}