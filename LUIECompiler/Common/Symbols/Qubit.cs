using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.CodeGeneration.Definitions;
using LUIECompiler.Common.Errors;

namespace LUIECompiler.Common.Symbols
{
    /// <summary>
    /// Represents a qubit. A qubit is a register of size 1.
    /// </summary>
    public class Qubit : Register
    {
        /// <summary>
        /// Creates a new qubit.
        /// </summary>
        /// <param name="identifier">Identifier of the qubit.</param>
        /// <param name="errorContext">Context of the qubit definition.</param>
        public Qubit(string identifier, ErrorContext errorContext) : base(identifier: identifier, size: 1, errorContext)
        {
        }

        public override string ToString()
        {
            return $"Qubit: {{ Identifier = {Identifier} }}";
        }

        /// <summary>
        /// Converts the qubit to a QASM parameter.
        /// </summary>
        /// <param name="definition"></param>
        /// <returns> The QASM parameter. </returns>
        public virtual QubitCode ToQASMCode(RegisterDefinition definition, CodeGenerationContext codeGenContext, ErrorContext context)
        {
            return new()
            {
                Register = definition,
                Identifier = codeGenContext.CurrentBlock.GetUniqueIdentifier(definition),
            };
        }
    }

}