using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Statements
{
    /// <summary>
    /// Represents a (predefined) gate application statement.
    /// </summary>
    public class GateApplicationStatement : Statement
    {
        /// <summary>
        /// Gate that is applied in the statement.
        /// </summary>
        public required Gate Gate { get; init; }

        /// <summary>
        /// Register to which the gate is applied to.
        /// </summary>
        public required List<Symbol> Parameters { get; init; }

        /// <summary>
        /// Returns the QASM code for the statement.
        /// </summary>
        /// <returns></returns>
        public override QASMProgram ToQASM(CodeGenerationContext context)
        {
            return new(new GateApplicationCode
            (
                gate: Gate.ToGateCode(context),
                guards: [],
                parameters: GetParameters(context)
            ));
        }

        /// <summary>
        /// Gets the parameters of the gate.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="CodeGenerationException"></exception>
        public List<QubitCode> GetParameters(CodeGenerationContext context)
        {
            return Parameters.Select(param =>
                TranslateQubit(param, context)
            ).ToList();
        }
    }

}