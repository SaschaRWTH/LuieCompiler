using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Statements
{
    /// <summary>
    /// Represents a composite gate statement.
    /// </summary>
    public class CompositeGateStatement : Statement
    {
        /// <summary>
        /// Gate that is applied in the statement.
        /// </summary>
        public required CompositeGate Gate { get; init; }

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
            Dictionary<Parameter, Symbol> parameterMap = context.ParameterMap;
            foreach (var parameter in Parameters)
            {
                parameterMap[parameter.Key] = parameter.Value;
            }

            CodeGenerationContext bodyContext = new CodeGenerationContext(context.IntegerConstants, parameterMap)
            {
                CurrentBlock = context.CurrentBlock,
                SymbolTable = context.SymbolTable,
            };

            return Gate.Body.ToQASM(bodyContext);
        }
    }
}