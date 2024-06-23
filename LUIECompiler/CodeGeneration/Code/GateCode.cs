using LUIECompiler.CodeGeneration.Definitions;
using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration.Code
{
    public class GateCode : AbstractCode 
    {
        public required List<string> Guards { get; init; }
        public required Gate Gate { get; init; }
        // TODO: extend to multiple params
        public required RegisterDefinition Register { get; init; }  

        public override string ToCode()
        {
            if(Guards.Count == 0){
                return $"{Gate} {Register.Identifier};";
            }

            return $"ctrl({Guards.Count}) @ {Gate} {string.Join(", ", Guards)}, {Register.Identifier};";
        } 
    }
}