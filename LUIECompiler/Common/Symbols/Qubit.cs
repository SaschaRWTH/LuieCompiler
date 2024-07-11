using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.CodeGeneration.Definitions;
using LUIECompiler.Common.Errors;

namespace LUIECompiler.Common.Symbols
{

    public class Qubit : Register
    {
        public Qubit(string identifier, ErrorContext errorContext) : base(identifier: identifier, size: 1, errorContext)
        {
        }

        public override string ToString()
        {
            return $"Qubit: [Identifier = {Identifier}]";
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