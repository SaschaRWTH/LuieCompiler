using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.CodeGeneration.Gates;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Statements { 
    public class CompositeGateStatement : Statement
    {
        /// <summary>
        /// Gate that is applied in the statement.
        /// </summary>
        public required DefinedGate Gate { get; init; }

        /// <summary>
        /// Register to which the gate is applied to.
        /// </summary>
        public required Dictionary<Parameter, Symbol> Parameters { get; init; }

        /// <summary>
        /// Returns the QASM code for the statement.
        /// </summary>
        /// <returns></returns>
        public override QASMProgram ToQASM(CodeGenerationContext context)
        {

            throw new NotImplementedException();
        }
    }
}