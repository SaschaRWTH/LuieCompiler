
using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration.Statements
{
    public abstract class QuantumIfStatment : AbstractStatement
    {
        public required RegisterInfo Guard { get; init; }

        public required CodeBlock Block { get; init; }

        public QuantumIfStatment(RegisterInfo register, CodeBlock block)
        {
            Guard = register;
            Block = block;
        }

        public override QASMCode ToQASM()
        {
            throw new NotImplementedException();
        }
    }

}