using LUIECompiler.CodeGeneration.Definitions;

namespace LUIECompiler.CodeGeneration.Codes
{
    public class DefinitionCode : Code 
    {
        // TODO: extend by size
        /// <summary>
        /// Register that is defined.
        /// </summary>
        public required RegisterDefinition Register { get; init; }  

        public override string ToCode()
        {
            return $"qubit {Register.Identifier};";
        } 
    }
}