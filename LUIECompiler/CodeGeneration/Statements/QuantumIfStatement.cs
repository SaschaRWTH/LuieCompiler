
using LUIECompiler.Common;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common.Symbols;
using LUIECompiler.CodeGeneration.Definitions;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Errors;

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

        public override QASMProgram ToQASM(List<Constant<int>> constants)
        {
            QubitCode qubit = GetGuardCode(constants);
            return Block.ToQASM(constants).AddGuard(qubit: qubit);
        }

        /// <summary>
        /// Gets string representation of the guard.
        /// </summary>
        /// <returns></returns>
        protected QubitCode GetGuardCode(List<Constant<int>> constants)
        {
            RegisterDefinition definition = GetDefinition(Guard) as RegisterDefinition ??
                throw new InternalException()
                {
                    Reason = "Guard is not a register definition. This should have been caught by the semantic analysis and type checking while generating."
                };

            return Guard.ToQASMCode(definition, constants, Line);
        }
    }

}