using LUIECompiler.CodeGeneration.Definitions;

namespace LUIECompiler.CodeGeneration.Code
{
    public class DefinitionCode : AbstractCode 
    {
        // TODO: extend by size
        public required RegisterDefinition Register { get; init; }  

        public override string ToCode()
        {
            return $"qubit {Register.Identifier};";
        } 
    }
}