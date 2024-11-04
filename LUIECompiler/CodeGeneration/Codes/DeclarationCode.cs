using LUIECompiler.CodeGeneration.Declarations;

namespace LUIECompiler.CodeGeneration.Codes
{
    public abstract class DeclarationCode : Code
    {
        /// <summary>
        /// Unique identifier of the register.
        /// </summary>
        public required UniqueIdentifier Identifier { get; init; }
        
        public override bool SemanticallyEqual(Code code)
        {
            if (code is not DeclarationCode definitionCode)
            {
                return false;
            }

            return Identifier == definitionCode.Identifier;
        }

    }
}