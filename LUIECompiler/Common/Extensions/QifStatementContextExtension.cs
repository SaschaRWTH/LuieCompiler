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

            if(symbol is GateArgument arg)
            {
                return GetGuardArgument(context.register(), arg);
            }

            if (symbol is not Register register)
            {
                Compiler.LogError($"Could not get the symbol of guard identifier '{identifier}'. Symbol is not a register.");
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
                Compiler.LogError($"Could not get the index expression of register '{identifier}'. Symbol is not a qubit.");
                throw new CodeGenerationException()
                {
                    Error = new TypeError(new ErrorContext(context.Start), identifier, typeof(Qubit), symbol.GetType()),
                };
            }
            index.PropagateSymbolInformation(table);

            return new RegisterAccess(register, index, new ErrorContext(context));
        }

        /// <summary>
        /// Gets the guard argument of the qif statement.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static GateArgument GetGuardArgument(LuieParser.RegisterContext context, GateArgument arg)
        {
            if (!context.TryGetIndexExpression(out Expression<int> index))
            {
                return arg;
            }

            return new GateArgumentAccess(arg, index, new ErrorContext(context));
        }
    }
}