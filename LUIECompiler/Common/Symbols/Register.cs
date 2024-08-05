using LUIECompiler.CodeGeneration.Expressions;
using LUIECompiler.Common.Errors;

namespace LUIECompiler.Common.Symbols
{
    /// <summary>
    /// Represents a register.
    /// </summary>
    public class Register : Symbol
    {
        /// <summary>
        /// The size of the register, i.e., the number of qubits it contains.
        /// </summary>
        public Expression<int> Size { get; init; }

        /// <summary>
        /// Creates a new register.
        /// </summary>
        /// <param name="identifier">Identifier of the register.</param>
        /// <param name="size">Size of the register.</param>
        /// <param name="errorContext">Error context of the register definition.</param>
        public Register(string identifier, Expression<int> size, ErrorContext errorContext) : base(identifier, errorContext)
        {
            Size = size;
        }

        /// <summary>
        /// Creates a new register.
        /// </summary>
        /// <param name="identifier">Identifier of the register.</param>
        /// <param name="size">Size of the register.</param>
        /// <param name="errorContext">Error context of the register definition.</param>
        protected Register(string identifier, int size, ErrorContext errorContext) : base(identifier, errorContext)
        {
            Size = new ConstantExpression<int>()
            {
                Value = size,
            };
        }

        public override string ToString()
        {
            return $"Register: {{ Identifier = {Identifier}, size = {Size} }}";
        }

        /// <summary>
        /// Creates a new <see cref="RegisterAccess"/> for this register given an index <paramref name="indexExpression"/>.
        /// </summary>
        /// <param name="indexExpression">Expression for the index to access.</param>
        /// <param name="errorContext"></param>
        /// <returns></returns>
        public RegisterAccess ToRegisterAccess(Expression<int> indexExpression, ErrorContext? errorContext = null)
        {
            return new RegisterAccess(this, indexExpression, errorContext ?? ErrorContext);
        }
    }

}