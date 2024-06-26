
using LUIECompiler.Common;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Statements
{
    public class QuantumIfStatement : Statement
    {
        /// <summary>
        /// Guard of the <see cref="Block"/>.
        /// </summary>
        public required Qubit Guard { get; init; }

        /// <summary>
        /// Nexted block of the if statement.
        /// </summary>
        public required CodeBlock Block { get; init; }

        public override QASMProgram ToQASM()
        {
            return Block.ToQASM().AddControl(identifier: IdentifierString());
        }

        protected string IdentifierString()
        {
            if (Guard is RegisterAccess access)
            {
                return $"{GetIdentifier(Guard)}[{access.Index}]";
            }

            return $"{GetIdentifier(Guard)}";

        }
    }

}