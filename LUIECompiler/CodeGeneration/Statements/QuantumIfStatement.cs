
using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration.Statements
{
    public class QuantumIfStatement : AbstractStatement
    {
        public required RegisterInfo Guard { get; init; }

        public required CodeBlock Block { get; init; }

        public QuantumIfStatement() {}

        public QuantumIfStatement(RegisterInfo register, CodeBlock block)
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