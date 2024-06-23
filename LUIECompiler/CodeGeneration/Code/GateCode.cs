using LUIECompiler.CodeGeneration.Definitions;
using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration.Code
{
    public class GateCode : AbstractCode 
    {
        public required List<RegisterDefinition> Guards { get; init; }
        public required Gate Gate { get; init; }
        // TODO: extend to multiple params
        public required RegisterDefinition Register { get; init; }  

        public override string ToCode()
        {
            throw new NotImplementedException();
        } 
    }
}