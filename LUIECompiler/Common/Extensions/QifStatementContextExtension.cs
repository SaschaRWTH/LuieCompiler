using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.CodeGeneration.Expressions;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.Common.Extensions
{
    public static class QifStatementContextExtension
    {
        /// <summary>
        /// Gets the guard of the qif statement.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        /// <exception cref="CodeGenerationException"></exception>
        public static Qubit GetGuard(this LuieParser.QifStatementContext context, SymbolTable table)
        {
            string identifier = context.register().GetIdentifier();
            Symbol symbol = table.GetSymbolInfo(identifier) ?? throw new CodeGenerationException()
            {
                Error = new UndefinedError(context.Start.Line, identifier),
            };

            if (symbol is not Register register)
            {
                throw new CodeGenerationException()
                {
                    Error = new TypeError(context.Start.Line, identifier, typeof(Register), symbol.GetType()),
                };
            }

            if (symbol is Qubit qubit)
            {
                return qubit;
            }

            if (!context.register().TryGetIndex(out int index))
            {
                throw new CodeGenerationException()
                {
                    Error = new TypeError(context.Start.Line, identifier, typeof(Qubit), symbol.GetType()),
                };
            }

            ConstantExpression<int> exp = new()
            {
                Value = index,
            };
            return new RegisterAccess(register, exp);
        }
    }
}