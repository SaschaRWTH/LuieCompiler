using LUIECompiler.Common;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Definitions
{

    public class RegisterDefinition : Definition
    {
        public override Symbol Register { get; init; }

        public RegisterDefinition(Register register)
        {
            Register = register;
        }

        public override QASMProgram ToQASM(CodeGenerationContext context)
        {
            Register register = (Register)Register;

            return new(new DefinitionCode()
            {
                Register = this,
                Size = register.Size.Evaluate(context.IntegerConstants),
                Identifier = context.CurrentBlock.GetUniqueIdentifier(this),
            });
        }
    }

}