
using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration.Statements
{
    public abstract class QuantumIfStatment : AbstractStatement
    {
        public required RegisterInfo Guard { get; init; }

        public required List<AbstractStatement> Block { get; init; }

        public QuantumIfStatment(RegisterInfo register, List<AbstractStatement> block)
        {
            Guard = register;
            Block = block;
        }

        public override string ToQASM()
        {
            throw new NotImplementedException();
        }
    }

}