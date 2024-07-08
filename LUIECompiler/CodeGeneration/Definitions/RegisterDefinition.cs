using LUIECompiler.Common;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Definitions
{

    public class RegisterDefinition : Definition
    {
        /// <summary>
        /// The size of the register.
        /// </summary>
        public required int Size { get; init; }

        public override QASMProgram ToQASM(List<Constant<int>> constants)
        {
            return new(new DefinitionCode()
            {
                Register = this,
                Size = Size,
            });
        }
    }

}