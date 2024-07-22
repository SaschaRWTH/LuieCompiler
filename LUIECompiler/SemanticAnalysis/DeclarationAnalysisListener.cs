using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using LUIECompiler.Common;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Extensions;
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

        public override void ExitRegisterDeclaration([NotNull] LuieParser.RegisterDeclarationContext context)
        {
            ITerminalNode id = context.IDENTIFIER();
            string identifier = id.GetText();
            if (Table.IsDefinedInCurrentScop(identifier))
            {
                Error.Report(new RedefineError(new ErrorContext(context.Start), identifier));
            }
            else
            {
                Qubit info = new(identifier, new ErrorContext(context));
                Table.AddSymbol(info);
            }
        }

        public override void ExitStatement([NotNull] LuieParser.StatementContext context)
        {
            base.ExitStatement(context);
        }

        public override void ExitGateapplication([NotNull] LuieParser.GateapplicationContext context)
        {
            List<string> identifiers = context.register().GetIdentifiers().ToList();
            foreach (string identifier in identifiers)
            {
                CheckDefinedness(identifier, context);
            }
        }

        public override void EnterQifStatement([NotNull] LuieParser.QifStatementContext context)
        {
            string identifier = context.register().GetIdentifier();
            CheckDefinedness(identifier, context);
        }
        
        public override void EnterForstatement([NotNull] LuieParser.ForstatementContext context)
        {

            string identifier = context.IDENTIFIER().GetText();
            

            if(Table.IsDefined(identifier))
            {
                Error.Report(new RedefineError(new ErrorContext(context.Start), identifier));
                return;
            }
            
            LuieParser.RangeContext range = context.range();
            LoopIterator loop = range.GetRange(identifier);
         
            Table.AddSymbol(loop);
        }

        public override void ExitFactor([NotNull] LuieParser.FactorContext context)
        {
            if(context.identifier?.Text is not string identifier)
            {
                return;
            }
            
            CheckDefinedness(identifier, context);    
            base.ExitFactor(context);
        }

        public override void ExitFunction([NotNull] LuieParser.FunctionContext context)
        {
            string identifier = context.param.Text;
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

            Error.Report(new UndefinedError(new ErrorContext(context.Start), identifier));
        }
    }

}
