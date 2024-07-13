using LUIECompiler.Common;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common.Symbols;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Errors;

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

            int size = register.Size.Evaluate(context.IntegerConstants);

            if (size <= 0)
            {
                throw new CodeGenerationException()
                {
                    Error = new InvalidSizeError(Register.ErrorContext, register.Identifier, size),
                };
            }

            return new(new DefinitionCode()
            {
                Register = this,
                Size = size,
                Identifier = context.CurrentBlock.GetUniqueIdentifier(this),
            });
        }
    }

}