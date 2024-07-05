using System.Runtime.CompilerServices;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.Common
{
    public static class ContextExtensions
    {
        /// <summary>
        /// Gets the identifier of the register.
        /// </summary>
        /// <param name="context">Context of the register.</param>
        /// <returns> Identifier of the register. </returns>
        public static string GetIdentifier(this LuieParser.RegisterContext context)
        {
            return context.IDENTIFIER().GetText();
        }

        /// <summary>
        /// Gets the identifiers of all registers.
        /// </summary>
        /// <param name="context"> Contexts of the registers. </param>
        /// <returns> Identifiers of the registers. </returns>
        public static IEnumerable<string> GetIdentifiers(this IEnumerable<LuieParser.RegisterContext> context)
        {
            return context.Select(c => c.GetIdentifier());
        }

        /// <summary>
        /// Checks whether an index is given in the context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool HasIndex(this LuieParser.RegisterContext context)
        {
            return context.index != null;
        }

        /// <summary>
        /// Gets the index in the context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="InternalException"></exception>
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

        /// <summary>
        /// Tries to get the index that is accessed in the register.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="index"> Index that is accessed. </param>
        /// <returns>True, if a valid <paramref name="index"/> was given, overwise false.</returns>
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

        /// <summary>
        /// Checks whether a size is given in the context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool HasSize(this LuieParser.DeclarationContext context)
        {
            return context.size != null;
        }

        /// <summary>
        /// Gets the size in the context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="InternalException"></exception>
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

        /// <summary>
        /// Tries to get the size of the register.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="size"> Size of the register. </param>
        /// <returns> True, if a valid <paramref name="size"/> was given, overwise false. </returns>
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

            return new RegisterAccess(register, index);
        }

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

            return new RegisterAccess(register, index);
        }

        public static LoopIterator GetIterator(this LuieParser.ForstatementContext context)
        {
            string identifier = context.IDENTIFIER().GetText();

            LuieParser.RangeContext range = context.range();
            if (!int.TryParse(range.start.Text, out int start) || !int.TryParse(range.end.Text, out int end))
            {
                throw new InternalException()
                {
                    Reason = "Failed to parse the range of the for statement.",
                };
            }

            // TODO: Add invalid range check

            return new(identifier, start, end);
        }
    }
}