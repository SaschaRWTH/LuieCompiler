using LUIECompiler.Common;
using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.CodeGeneration.Definitions
{

    public class RegisterDefinition : Definition
    {
        /// <summary>
        /// The size of the register.
        /// </summary>
        public required int Size { get; init; }

        public override QASMProgram ToQASM()
        {
            return new(new DefinitionCode()
            {
                Register = this,
                Size = Size,
            });
        }
    }

}