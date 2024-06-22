using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration.Definitions
{

    public abstract class RegisterDefinition : AbstractDefinition
    {
        // Not yet in grammar 
        // public required int Size { get; init; }

        public RegisterDefinition(string identifier)
        {
            Identifier = identifier;
        }

        public override QASMCode ToQASM()
        {
            throw new NotImplementedException();
        }
    }

}