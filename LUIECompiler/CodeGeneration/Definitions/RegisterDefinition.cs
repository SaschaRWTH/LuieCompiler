using LUIECompiler.Common;
using LUIECompiler.CodeGeneration.Code;

namespace LUIECompiler.CodeGeneration.Definitions
{

    public class RegisterDefinition : AbstractDefinition
    {
        // Not yet in grammar 
        // public required int Size { get; init; }

        public override QASMCode ToQASM()
        {
            return new($"qubit {Identifier};");
        }
    }

}