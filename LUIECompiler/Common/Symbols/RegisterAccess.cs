using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.CodeGeneration.Definitions;
using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Expressions;

namespace LUIECompiler.Common.Symbols
{

    public class RegisterAccess : Qubit
    {
        /// <summary>
        /// Expression that evaluates to the index of the qubit in the <see cref="Register"/>.
        /// </summary>
        public Expression<int> IndexExpression { get; init; }

        /// <summary>
        /// Index of the qubit in the <see cref="Register"/>.
        /// </summary>
        public int Index { get => IndexExpression.Evaluate() ;}

        /// <summary>
        /// Register that is accessed.
        /// </summary>
        public Register Register { get; init; }

        public RegisterAccess(Register register, Expression<int> indexExpression) : base(identifier: register.Identifier) 
        {
            IndexExpression = indexExpression;
            Register = register;
        }

        public override QubitCode ToQASMCode(RegisterDefinition definition)
        {
            return new RegisterAccessCode()
            {
                Register = definition,
                Index = this.Index,
            };
        }
    }

}