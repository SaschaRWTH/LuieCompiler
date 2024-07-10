
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

        public override QASMProgram ToQASM(CodeGenerationContext context)
        {
            QubitCode qubit = GetGuardCode(context);
            return Block.ToQASM(context).AddGuard(qubit: qubit);
        }

        /// <summary>
        /// Gets string representation of the guard.
        /// </summary>
        /// <returns></returns>
        protected QubitCode GetGuardCode(CodeGenerationContext context)
        {
            return TranslateQubit(Guard, context);
        }
    }

}