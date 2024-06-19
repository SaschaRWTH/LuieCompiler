using System.Data.Common;
using System.Diagnostics;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using LUIECompiler.Common;
using LUIECompiler.Common.Errors;

namespace LUIECompiler.SemanticAnalysis
{
    public class DeclarationAnalysisListener : LuieBaseListener
    {
        private readonly SymbolTable table = new();

        /// <summary>
        /// Error handler of the listener.
        /// </summary>
        public ErrorHandler Error { get; init; } = new();

        public override void EnterBlock([NotNull] LuieParser.BlockContext context)
        {

            base.EnterBlock(context);
        }

        public override void ExitBlock([NotNull] LuieParser.BlockContext context)
        {
            base.ExitBlock(context);
        }

        public override void ExitDeclaration([NotNull] LuieParser.DeclarationContext context)
        {
            ITerminalNode id = context.IDENTIFIER();
            string identifier = id.GetText();
            if (table.IsDefined(identifier))
            {
                Error.Report(new RedefineError(context.Start.Line, identifier));
            }
            else
            {
                RegisterInfo info = new(identifier);
                table.AddSymbol(info);
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

        public override void ExitIfStat([NotNull] LuieParser.IfStatContext context)
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
            if (table.IsDefined(identifier))
            {
                return;
            }
            
            Error.Report(new UndefinedError(context.Start.Line, identifier));
        }

    }

}
