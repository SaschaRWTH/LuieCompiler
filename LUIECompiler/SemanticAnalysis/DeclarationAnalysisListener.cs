using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using LUIECompiler.Common;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.SemanticAnalysis
{
    public class DeclarationAnalysisListener : LuieBaseListener
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
            if (Table.IsDefinedInCurrentScop(identifier))
            {
                Error.Report(new RedefineError(context.Start.Line, identifier));
            }
            else
            {
                Qubit info = new(identifier);
                Table.AddSymbol(info);
            }

            base.ExitDeclaration(context);
        }

        public override void ExitStatement([NotNull] LuieParser.StatementContext context)
        {
            base.ExitStatement(context);
        }

        public override void ExitGateapplication([NotNull] LuieParser.GateapplicationContext context)
        {
            var node = context.IDENTIFIER();
            string identifier = node.GetText();
            CheckDefinedness(identifier, context);
            // Could check type here, or create new TypeCheckListener
        }

        public override void EnterQifStatement([NotNull] LuieParser.QifStatementContext context)
        {
            var node = context.IDENTIFIER();
            string identifier = node.GetText();
            CheckDefinedness(identifier, context);
        }

        /// <summary>
        /// Checks whether a given <paramref name="identifier"/> is defined in the current context.
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="context"></param>
        private void CheckDefinedness(string identifier, ParserRuleContext context)
        {
            if (Table.IsDefined(identifier))
            {
                return;
            }
            
            Error.Report(new UndefinedError(context.Start.Line, identifier));
        }

    }

}
