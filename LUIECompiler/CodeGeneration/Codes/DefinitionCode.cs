using LUIECompiler.CodeGeneration.Definitions;

namespace LUIECompiler.CodeGeneration.Codes
{
    public class DefinitionCode : Code
    {
        /// <summary>
        /// Register that is defined.
        /// </summary>
        public required RegisterDefinition Register { get; init; }

        /// <summary>
        /// The size of the register.
        /// </summary>
        public required int Size { get; init; }

        public override string ToCode()
        {
            if (Size == 1)
            {
                return $"qubit {Register.Identifier};";
            }

            return $"qubit[{Size}] {Register.Identifier};";
        }
    }
}