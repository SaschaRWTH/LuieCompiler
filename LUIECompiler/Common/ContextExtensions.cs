using System.Runtime.CompilerServices;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.Common
{
    public static class ContextExtensions
    {
        public static string GetIdentifier(this LuieParser.RegisterContext context)
        {
            return context.IDENTIFIER().GetText();
        }

        public static IEnumerable<string> GetIdentifiers(this IEnumerable<LuieParser.RegisterContext> context)
        {
            return context.Select(c => c.GetIdentifier());
        }

        public static bool HasIndex(this LuieParser.RegisterContext context)
        {
            return context.index != null;
        }

        public static int GetIndex(this LuieParser.RegisterContext context)
        {
            if (!int.TryParse(context?.index?.Text, out int result))
            {
                throw new InternalException()
                {
                    Reason = "Could not parse the index. This should not occure!"
                };
            }
            return result;
        }

        public static bool TryGetIndex(this LuieParser.RegisterContext context, out int index)
        {
            index = 0;

            if (!int.TryParse(context?.index?.Text, out int result))
            {
                return false;
            }
            index = result;
            return true;
        }

        public static bool HasSize(this LuieParser.DeclarationContext context)
        {
            return context.size != null;
        }

        public static int GetSize(this LuieParser.DeclarationContext context)
        {
            if (!int.TryParse(context?.size?.Text, out int result))
            {
                throw new InternalException()
                {
                    Reason = "Could not parse the index. This should not occure!"
                };
            }
            return result;
        }

        public static bool TryGetSize(this LuieParser.DeclarationContext context, out int size)
        {
            size = 0;

            if (!int.TryParse(context?.size?.Text, out int result))
            {
                return false;
            }
            size = result;
            return true;
        }

        public static List<Qubit> GetParameter(this LuieParser.GateapplicationContext context, SymbolTable table)
        {
            List<LuieParser.RegisterContext> registers = context.register().ToList();
            return registers.Select(register => SingleParameter(register, table)).ToList();
        }

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
                    Error = new TypeError(context.Start.Line, identifier),
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
                    Error = new TypeError(context.Start.Line, identifier),
                };
            }

            return new RegisterAccess(register, index);
        }

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
                    Error = new TypeError(context.Start.Line, identifier),
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
                    Error = new TypeError(context.Start.Line, identifier),
                };
            }

            return new RegisterAccess(register, index);
        }
    }
}