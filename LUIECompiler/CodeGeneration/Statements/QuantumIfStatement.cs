
using LUIECompiler.Common;
using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.CodeGeneration.Statements
{
    public class QuantumIfStatement : Statement
    {
        /// <summary>
        /// Guard of the <see cref="Block"/>.
        /// </summary>
        public required RegisterInfo Guard { get; init; }

        /// <summary>
        /// Nexted block of the if statement.
        /// </summary>
        public required CodeBlock Block { get; init; }

        public override QASMProgram ToQASM()
        {
            return Block.ToQASM().AddControl(identifier: GetIdentifier(Guard));
        }
    }

}