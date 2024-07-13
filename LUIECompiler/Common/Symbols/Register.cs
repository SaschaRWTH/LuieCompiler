using LUIECompiler.CodeGeneration.Expressions;
using LUIECompiler.Common.Errors;

namespace LUIECompiler.Common.Symbols
{

    public class Register : Symbol
    {
        /// <summary>
        /// The size of the register, i.e., the number of qubits it contains.
        /// </summary>
        public Expression<int> Size { get; init; }

        public Register(string identifier, Expression<int> size, ErrorContext errorContext) : base(identifier, errorContext)
        {
            Size = size;
        }
        protected Register(string identifier, int size, ErrorContext errorContext) : base(identifier, errorContext)
        {
            Size = new ConstantExpression<int>()
            {
                Value = size,
            };
        }

        public override string ToString()
        {
            return $"Register: [Identifier = {Identifier}, size = {Size}]";
        }
    }

}