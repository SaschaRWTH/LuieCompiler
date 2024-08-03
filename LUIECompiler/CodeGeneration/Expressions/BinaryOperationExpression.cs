using System.Numerics;
using LUIECompiler.Common;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Expressions
{

    public class BinaryOperationExpression<T> : Expression<T> where T : INumber<T>
    {
        /// <summary>
        /// Left operand of the binary operation.
        /// </summary>
        public required Expression<T> Left { get; init; }

        /// <summary>
        /// Right operand of the binary operation.
        /// </summary>
        public required Expression<T> Right { get; init; }

        /// <summary>
        /// Operator of the binary operation.
        /// </summary>
        public required BinaryOperator<T> Operator { get; init; }

        public override T Evaluate(CodeGenerationContext context) 
        { 
            return Operator.Apply(Left.Evaluate(context), Right.Evaluate(context));
        }

        public override List<string> UndefinedIdentifiers(SymbolTable table)
        {
            return [..Left.UndefinedIdentifiers(table), ..Right.UndefinedIdentifiers(table)];
        }
    }
}