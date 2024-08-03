using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.CodeGeneration.Expressions;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.Common.Extensions
{
    public static class GateApplicationContext
    {
        /// <summary>
        /// Gets the parameters of the gate application.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static List<Symbol> GetParameters(this LuieParser.GateapplicationContext context, SymbolTable table)
        {
            var registers = context.register();
            return registers.Select(register => SingleSymbol(register, table)).ToList();
        }

        /// <summary>
        /// Gets a single parameter of a gate application based on the <see cref="LuieParser.RegisterContext"/> <paramref name="context"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        /// <exception cref="CodeGenerationException"></exception>
        private static Symbol SingleSymbol(LuieParser.RegisterContext context, SymbolTable table)
        {
            string identifier = context.GetIdentifier();

            Symbol symbol = table.GetSymbolInfo(identifier) ?? throw new CodeGenerationException()
            {
                Error = new UndefinedError(new ErrorContext(context.Start), identifier),
            };

            if (symbol is Parameter parameter)
            {
                return SingleParameter(context, parameter, table);
            }

            if (symbol is not Register register)
            {
                throw new CodeGenerationException()
                {
                    Error = new TypeError(new ErrorContext(context.Start), identifier, typeof(Register), symbol.GetType()),
                };
            }

            return SingleRegister(context, register, table);
        }

        /// <summary>
        /// Gets the symbol of a single register.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="register"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        /// <exception cref="InternalException"></exception>
        /// <exception cref="CodeGenerationException"></exception>
        private static Register SingleRegister(LuieParser.RegisterContext context, Register register, SymbolTable table)
        {

            if (register is Qubit qubit)
            {
                return qubit;
            }

            if (context.index is null)
            {
                return register;
            }

            if (!context.TryGetIndexExpression(out Expression<int> index))
            {
                throw new InternalException()
                {
                    Reason = $"Could not get index expression for register {register.Identifier}.",
                };
            }

            List<string> undefined = index.UndefinedIdentifiers(table);
            if (undefined.Count > 0)
            {
                throw new CodeGenerationException()
                {
                    Error = new UndefinedError(new ErrorContext(context.Start), undefined),
                };
            }

            return new RegisterAccess(register, index, new ErrorContext(context));
        }

        /// <summary>
        /// Gets the symbol of a single parameter.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parameter"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        private static Parameter SingleParameter(LuieParser.RegisterContext context, Parameter parameter, SymbolTable table)
        {
            if (!context.TryGetIndexExpression(out Expression<int> index))
            {
                return parameter;
            }

            return new ParameterAccess(parameter, index, new ErrorContext(context));
        }
    }
}