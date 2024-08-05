using System.Numerics;
using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration.Expressions
{
    /// <summary>
    /// Represents a unary operation expression.
    /// </summary>
    /// <typeparam name="T">Type of the result.</typeparam>
    public class UnaryOperationExpression<T> : Expression<T> where T : INumber<T>
    {
        /// <summary>
        /// Left operand of the binary operation.
        /// </summary>
        public required Expression<T> Operand { get; init; }

        /// <summary>
        /// Operator of the binary operation.
        /// </summary>
        public required UnaryOperator<T> Operator { get; init; }

        public override T Evaluate(CodeGenerationContext context) 
        { 
            return Operator.Apply(Operand.Evaluate(context));
        }

        public override List<string> UndefinedIdentifiers(SymbolTable table)
        {
            return [..Operand.UndefinedIdentifiers(table)];
        }

        public override string ToString()
        {
            return $"{Operator} {Operand}";
        }
    }
}