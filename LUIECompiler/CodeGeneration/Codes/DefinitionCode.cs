using LUIECompiler.CodeGeneration.Definitions;

namespace LUIECompiler.CodeGeneration.Codes
{
    public class DefinitionCode : Code
    {
        /// <summary>
        /// Unique identifier of the register.
        /// </summary>
        public required UniqueIdentifier Identifier { get; init; }

        /// <summary>
        /// The size of the register.
        /// </summary>
        public required int Size { get; init; }

        public override bool SemanticallyEqual(Code code)
        {
            if (code is not DefinitionCode definitionCode)
            {
                return false;
            }

            return Identifier == definitionCode.Identifier;
        }

        public override string ToCode()
        {
            if (Size == 1)
            {
                return $"qubit {Identifier.Identifier};";
            }

            return $"qubit[{Size}] {Identifier.Identifier};";
        }

        
    }
}