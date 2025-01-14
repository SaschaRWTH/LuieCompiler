using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.CodeGeneration.Declarations;
using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Expressions;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Errors;

namespace LUIECompiler.Common.Symbols
{
    /// <summary>
    /// Represents a register access.
    /// </summary>
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

        /// <summary>
        /// Creates a new register access.
        /// </summary>
        /// <param name="register">Register being accessed.</param>
        /// <param name="indexExpression">Expression that evaluates to the index of the qubit in the <see cref="Register"/>.</param>
        /// <param name="errorContext">Context of the register access.</param>
        public RegisterAccess(Register register, Expression<int> indexExpression, ErrorContext errorContext)
                       : base(identifier: register.Identifier, errorContext)
        {
            IndexExpression = indexExpression;
            Register = register;
        }

        public override QubitCode ToQASMCode(RegisterDeclaration definition, CodeGenerationContext codeGenContext, ErrorContext context)
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
                Register = definition.Symbol,
                Index = index,
                Identifier = codeGenContext.CurrentBlock.GetUniqueIdentifier(definition),
            };
        }

        public override string ToString()
        {
            return $"RegisterAccess: {{ Register = {Register}, Index = {IndexExpression} }}";
        }

        public override List<string> PropagateSymbolInformation(SymbolTable table)
        {
            return [.. base.PropagateSymbolInformation(table), .. IndexExpression.PropagateSymbolInformation(table)];
        }
    }

}