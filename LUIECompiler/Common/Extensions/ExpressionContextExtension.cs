using System.Numerics;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.CodeGeneration.Expressions;

namespace LUIECompiler.Common.Extensions
{
    public static class ExpressionContextExtension
    {
        /// <summary>
        /// Gets the expression from the <paramref name="context"/>.
        /// </summary>
        /// <typeparam name="T">Type of the result of the expression</typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="InternalException"></exception>
        public static Expression<T> GetExpression<T>(this LuieParser.ExpressionContext context, SymbolTable symbolTable) where T : INumber<T>
        {
            if(context.op is null)
            {
                return context.term().GetExpression<T>(symbolTable);
            }

            if(context.left is null || context.right is null)
            {
                throw new InternalException()
                {
                    Reason = "Binary operation expression must have both left and right operands if it has an operator.",
                };
            }

            BinaryOperator<T> op = BinaryOperator<T>.FromString(context.op.Text); 
            return new BinaryOperationExpression<T>()
            {
                Left = context.left.GetExpression<T>(symbolTable),
                Right = context.right.GetExpression<T>(symbolTable),
                Operator = op,
            };
        }
    }
}