using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.CodeGeneration.Expressions;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.Common.Extensions
{
    public static class GateApplicationContext
    {
        /// <summary>
        /// Gets the arguments of the gate application.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static List<Symbol> GetArguments(this LuieParser.GateapplicationContext context, SymbolTable table)
        {
            var registers = context.register();
            return registers.Select(register => SingleSymbol(register, table)).ToList();
        }

        /// <summary>
        /// Gets a single argument of a gate application based on the <see cref="LuieParser.RegisterContext"/> <paramref name="context"/>.
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

            if (symbol is GateArgument arg)
            {
                return SingleArgument(context, arg, table);
            }

            if (symbol is not Register register)
            {
                Compiler.LogError($"Could not get the symbol of identifier '{identifier}'. Symbol is not a register.");
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

            if (!context.TryGetIndexExpression(table, out Expression<int> index))
            {
                throw new InternalException()
                {
                    Reason = $"Could not get index expression for register {register.Identifier}.",
                };
            }

            List<string> undefined = index.PropagateSymbolInformation(table);
            if (undefined.Count > 0)
            {
                throw new CodeGenerationException()
                {
                    Error = new UndefinedError(new ErrorContext(context.Start), undefined),
                };
            }

            RegisterAccess access = new RegisterAccess(register, index, new ErrorContext(context));
            access.PropagateSymbolInformation(table);

            return access;
        }

        /// <summary>
        /// Gets the symbol of a single argument.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="arg"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        private static GateArgument SingleArgument(LuieParser.RegisterContext context, GateArgument arg, SymbolTable table)
        {
            if (!context.TryGetIndexExpression(table, out Expression<int> index))
            {
                return arg;
            }
            index.PropagateSymbolInformation(table);
            
            return new GateArgumentAccess(arg, index, new ErrorContext(context));
        }
    }
}