using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.CodeGeneration.Definitions;
using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Expressions;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Errors;

namespace LUIECompiler.Common.Symbols
{

    public class RegisterAccess : Qubit
    {
        /// <summary>
        /// Expression that evaluates to the index of the qubit in the <see cref="Register"/>.
        /// </summary>
        public Expression<int> IndexExpression { get; init; }

        /// <summary>
        /// Register that is accessed.
        /// </summary>
        public Register Register { get; init; }

        public RegisterAccess(Register register, Expression<int> indexExpression, ErrorContext errorContext) 
        : base(identifier: register.Identifier, errorContext)
        {
            IndexExpression = indexExpression;
            Register = register;
        }

        public override QubitCode ToQASMCode(RegisterDefinition definition, CodeGenerationContext codeGenContext, ErrorContext context)
        {
            int index = IndexExpression.Evaluate(codeGenContext);
            int size = Register.Size.Evaluate(codeGenContext);

            if (index < 0 || index >= size)
            {
                throw new CodeGenerationException()
                {
                    Error = new InvalidAccessError(context, Register.Identifier, index, size)
                };

            }

            return new RegisterAccessCode()
            {
                Register = definition,
                Index = index,
                Identifier = codeGenContext.CurrentBlock.GetUniqueIdentifier(definition),
            };
        }
    }

}