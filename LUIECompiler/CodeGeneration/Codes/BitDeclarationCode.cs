namespace LUIECompiler.CodeGeneration.Codes
{

    public class BitDeclarationCode : DeclarationCode
    {
        /// <summary>
        /// The size of the register.
        /// </summary>
        public required int Size { get; init; }

        public override string ToCode()
        {
            if (Size == 1)
            {
                return $"bit {Identifier.Identifier};";
            }

            return $"bit[{Size}] {Identifier.Identifier};";
        }

        
    }
}