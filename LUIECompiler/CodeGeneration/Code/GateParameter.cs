using LUIECompiler.CodeGeneration.Definitions;

namespace LUIECompiler.CodeGeneration.Codes
{
    public class GateParameter
    {
        public required RegisterDefinition Register { get; init; }
    
        public virtual string ToParameterCode()
        {
            return Register.Identifier;
        } 
    }
}