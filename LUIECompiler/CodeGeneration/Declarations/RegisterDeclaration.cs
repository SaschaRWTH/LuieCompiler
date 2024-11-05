using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common.Symbols;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Errors;

namespace LUIECompiler.CodeGeneration.Declarations
{
    /// <summary>
    /// Represents a definition of a register.
    /// </summary>
    public class RegisterDeclaration : Declaration
    {
        /// <summary>
        /// The register to define.
        /// </summary>
        public override Symbol Symbol { get; init; }

        public RegisterDeclaration(Register register)
        {
            Symbol = register;
        }

        public override QASMProgram ToQASM(CodeGenerationContext context)
        {
            Register register = (Register)Symbol;

            int size = register.Size.Evaluate(context);

            if (size <= 0)
            {
                throw new CodeGenerationException()
                {
                    Error = new InvalidSizeError(Symbol.ErrorContext, register.Identifier, size),
                };
            }

            UniqueIdentifier identifier = new(context.SymbolTable);
            context.CurrentBlock.AddIdentifier(this, identifier);

            return new(new QubitDeclarationCode()
            {
                Size = size,
                Identifier = identifier,
            });
        }
    }

}