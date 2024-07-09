using LUIECompiler.Common;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.CodeGeneration.Definitions;
using LUIECompiler.Common.Errors;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Statements
{
    public class GateApplicationStatement : Statement
    {
        /// <summary>
        /// Gate that is applied in the statement.
        /// </summary>
        public required Gate Gate { get; init; }

        /// <summary>
        /// Register to which the gate is applied to.
        /// </summary>
        public required List<Qubit> Parameters { get; init; }

        /// <summary>
        /// Returns the QASM code for the statement.
        /// </summary>
        /// <returns></returns>
        public override QASMProgram ToQASM(List<Constant<int>> constants)
        {
            return new(new GateCode()
            {
                Gate = Gate,
                Guards = [],
                Parameters = GetParameters(constants),
            });
        }

        /// <summary>
        /// Gets the parameters of the gate.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="CodeGenerationException"></exception>
        public List<QubitCode> GetParameters(List<Constant<int>> constants)
        {
            return Parameters.Select(param =>
                TranslateQubit(param, constants)
            ).ToList();
        }
    }

}