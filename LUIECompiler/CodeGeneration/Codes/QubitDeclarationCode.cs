namespace LUIECompiler.CodeGeneration.Codes
{

    public class QubitDeclarationCode : DeclarationCode
    {
        /// <summary>
        /// The size of the register.
        /// </summary>
        public required int Size { get; init; }

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