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
        public static List<Qubit> GetParameters(this LuieParser.GateapplicationContext context, SymbolTable table)
        {
            List<LuieParser.RegisterContext> registers = context.register().ToList();
            return registers.Select(register => SingleParameter(register, table)).ToList();
        }

        /// <summary>
        /// Gets a single parameter of a gate application based on the <see cref="LuieParser.RegisterContext"/> <paramref name="context"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        /// <exception cref="CodeGenerationException"></exception>
        private static Qubit SingleParameter(LuieParser.RegisterContext context, SymbolTable table)
        {
            string identifier = context.GetIdentifier();
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

            if (!context.TryGetIndex(out int index))
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