using LUIECompiler.CodeGeneration.Definitions;

namespace LUIECompiler.CodeGeneration.Codes
{
    public class RegisterAccessGateParameter : GateParameter
    {
        public required int Index { get; init; }
    
        public override string ToParameterCode()
        {
            return $"{Register.Identifier}[{Index}]";
        } 
    }
}