using System.Numerics;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.CodeGeneration.Expressions;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.Common.Extensions
{
    public static class ConstantContextExtension
    {
        /// <summary>
        /// Gets the constant symbol.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Symbol GetConstantSymbol(this LuieParser.ConstDeclarationContext context)
        {
            string typeString = context.type.Text;
            return typeString switch
            {
                "int" => GetConstant<int>(context),
                "double" => GetConstant<double>(context),
                "uint" => GetConstant<uint>(context),
                _ => throw new InternalException()
                {
                    Reason = $"Unknown constant type {typeString}. Grammar should only allow valid types.",
                }
            };
        }

        /// <summary>
        /// Gets the constant symbol depending on the type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Constant<T> GetConstant<T>(this LuieParser.ConstDeclarationContext context) where T : INumber<T>
        {
            string identifier = context.identifier.Text;
            Expression<T> value = context.GetConstantExpression<T>();
            return new Constant<T>(identifier, value, new ErrorContext(context));
        }

        /// <summary>
        /// Gets the expression of the constant.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Expression<T> GetConstantExpression<T>(this LuieParser.ConstDeclarationContext context) where T : INumber<T>
        {
            return context.exp.GetExpression<T>();
        }
    }
}