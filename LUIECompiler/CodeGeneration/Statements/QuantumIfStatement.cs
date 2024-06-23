
using LUIECompiler.Common;
using LUIECompiler.CodeGeneration.Code;

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
            return Block.ToQASM().AddControl(identifier: GetIdentifier(Guard));
        }
    }

}