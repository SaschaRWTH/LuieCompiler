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
            int index = IndexExpression.Evaluate(codeGenContext.IntegerConstants);

            if (index < 0 || index >= Register.Size)
            {
                throw new CodeGenerationException()
                {
                    Error = new InvalidAccessError(context, Register.Identifier, index, Register.Size)
                };

            }

            return new RegisterAccessCode()
            {
                Register = definition,
                Index = IndexExpression.Evaluate(codeGenContext.IntegerConstants),
            };
        }
    }

}