using System.Data.Common;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using LUIECompiler.Common;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.SemanticAnalysis
{
    public class TypeCheckListener : LuieBaseListener
    {
        /// <summary>
        /// Symbol table for the semantic analysis.
        /// </summary>
        public readonly SymbolTable Table = new();

        /// <summary>
        /// Error handler of the listener.
        /// </summary>
        public ErrorHandler Error { get; init; } = new();

        public override void EnterBlock([NotNull] LuieParser.BlockContext context)
        {
            Table.PushScope();
        }

        public override void ExitBlock([NotNull] LuieParser.BlockContext context)
        {
            Table.PopScope();
        }

        public override void ExitDeclaration([NotNull] LuieParser.DeclarationContext context)
        {
            ITerminalNode id = context.IDENTIFIER();
            string identifier = id.GetText();

            Register reg;
            if (context.TryGetSize(out int size))
            {
                reg = new Register(identifier, size);
            }
            else
            {
                reg = new Qubit(identifier);
            }
            Table.AddSymbol(reg);

            base.ExitDeclaration(context);
        }

        public override void ExitRegister([NotNull] LuieParser.RegisterContext context)
        {
            string identifier = context.IDENTIFIER().GetText();

            Symbol? symbol = Table.GetSymbolInfo(identifier);
            if (symbol == null)
            {
                Error.Report(new UndefinedError(context.Start.Line, identifier));
                return;
            }

            // Cannot access qubit with []
            if (symbol is Qubit && context.TryGetIndex(out int _))
            {
                Error.Report(new UndefinedError(context.Start.Line, identifier));
            }
        }

        public override void ExitGateapplication([NotNull] LuieParser.GateapplicationContext context)
        {
            List<string> identifiers = context.register().GetIdentifiers().ToList();

            foreach (string identifier in identifiers)
            {
                Symbol? symbol = Table.GetSymbolInfo(identifier);
                if (symbol == null)
                {
                    Error.Report(new UndefinedError(context.Start.Line, identifier));
                    return;
                }

                if (symbol is not Register)
                {
                    Error.Report(new TypeError(context.Start.Line, identifier));
                }
            }
        }

        public override void ExitQifStatement([NotNull] LuieParser.QifStatementContext context)
        {
            LuieParser.RegisterContext registerContext = context.register();
            string identifier = registerContext.GetIdentifier();

            Symbol? symbol = Table.GetSymbolInfo(identifier);
            if (symbol == null)
            {
                Error.Report(new UndefinedError(context.Start.Line, identifier));
                return;
            }

            if (symbol is Qubit)
            {
                return;
            }

            if (symbol is Register && registerContext.TryGetIndex(out int _))
            {
                return;
            }

            Error.Report(new TypeError(context.Start.Line, identifier));
        }
    }

}
