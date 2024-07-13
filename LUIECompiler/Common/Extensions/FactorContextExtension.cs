using System.Globalization;
using System.Numerics;
using Antlr4.Runtime;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.CodeGeneration.Expressions;

namespace LUIECompiler.Common.Extensions
{
    public static class FactorContextExtension
    {
        /// <summary>
        /// Gets the expression of the <see cref="LuieParser.ExpressionContext"/>.
        /// </summary>
        /// <typeparam name="T">Type of the result of the expression.</typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static Expression<T> GetExpression<T>(this LuieParser.FactorContext context) where T : INumber<T>
        {
            if(context.value is not null)
            {
                return GetConstantExpression<T>(context.value);
            }

            if(context.identifier is not null)
            {
                return new IdentifierExpression<T>()
                {
                    Identifier = context.identifier.Text,
                };
            }

            if(context.exp is not null)
            {
                return context.exp.GetExpression<T>();
            }

            LuieParser.FactorContext? factor = context.factor();
            if(factor is not null && context.op is not null)
            {
                UnaryOperator<T> op = UnaryOperator<T>.FromString(context.op.Text);
                return new UnaryOperationExpression<T>()
                {
                    Operand = factor.GetExpression<T>(),
                    Operator = op,
                };
            }
            
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the constant expression of the <see cref="IToken"/>.
        /// </summary>
        /// <typeparam name="T">Type of the result of the expression.</typeparam>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="InternalException"></exception>
        private static Expression<T> GetConstantExpression<T>(IToken token) where T : INumber<T>
        {
            if(!T.TryParse(token.Text, CultureInfo.InvariantCulture , out T? value) || value is null)
            {
                throw new InternalException()
                {
                    Reason = $"Failed to parse '{token.Text}' to constant value of type {typeof(T)}.",
                };
            }

            return new ConstantExpression<T>()
            {
                Value = value,
            };
        }
    }
}