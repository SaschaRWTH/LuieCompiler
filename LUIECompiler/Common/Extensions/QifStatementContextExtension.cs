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
        public static Symbol GetGuard(this LuieParser.QifStatementContext context, SymbolTable table)
        {
            string identifier = context.register().GetIdentifier();
            Symbol symbol = table.GetSymbolInfo(identifier) ?? throw new CodeGenerationException()
            {
                Error = new UndefinedError(new ErrorContext(context.Start), identifier),
            };

            if(symbol is Parameter parameter)
            {
                return GetGuardParameter(context.register(), parameter);
            }

            if (symbol is not Register register)
            {
                throw new CodeGenerationException()
                {
                    Error = new TypeError(new ErrorContext(context.Start), identifier, typeof(Register), symbol.GetType()),
                };
            }

            if (symbol is Qubit qubit)
            {
                return qubit;
            }

            if (!context.register().TryGetIndexExpression(out Expression<int> index))
            {
                throw new CodeGenerationException()
                {
                    Error = new TypeError(new ErrorContext(context.Start), identifier, typeof(Qubit), symbol.GetType()),
                };
            }
            
            return new RegisterAccess(register, index, new ErrorContext(context));
        }

        /// <summary>
        /// Gets the guard parameter of the qif statement.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static Parameter GetGuardParameter(LuieParser.RegisterContext context, Parameter parameter)
        {
            if (!context.TryGetIndexExpression(out Expression<int> index))
            {
                return parameter;
            }

            return new ParameterAccess(parameter, index, new ErrorContext(context));
        }
    }
}