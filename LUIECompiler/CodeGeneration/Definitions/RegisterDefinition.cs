using LUIECompiler.Common;
using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.CodeGeneration.Definitions
{

    public class RegisterDefinition : Definition
    {
        // Not yet in grammar 
        // public required int Size { get; init; }

        public override QASMProgram ToQASM()
        {
            return new(new DefinitionCode(){
                Register = this,
            });
        }
    }

}