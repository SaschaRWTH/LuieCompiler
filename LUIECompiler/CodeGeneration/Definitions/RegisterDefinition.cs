using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration.Definitions
{

    public class RegisterDefinition : AbstractDefinition
    {
        // Not yet in grammar 
        // public required int Size { get; init; }

        public override QASMCode ToQASM()
        {
            throw new NotImplementedException();
        }
    }

}