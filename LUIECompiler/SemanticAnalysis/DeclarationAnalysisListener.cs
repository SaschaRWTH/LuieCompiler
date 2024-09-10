using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.CodeGeneration.Expressions;
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

        public override void EnterMainblock([NotNull] LuieParser.MainblockContext context)
        {
            CodeBlock mainBlock = new()
            {
                Parent = null
            };
            Table.PushScope(mainBlock);
        }

        public override void ExitMainblock([NotNull] LuieParser.MainblockContext context)
        {
            // Technically not needed, just for completeness.
            Table.PopScope();
        }

        public override void EnterBlock([NotNull] LuieParser.BlockContext context)
        {
            Table.PushEmtpyScope();
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

            Symbol? info = Table.GetSymbolInfo(identifier);
            if (info is null)
            {
                Error.Report(new UndefinedError(new ErrorContext(context.Start), identifier));
            }

            // Push potentially null guard such that poping stack does not mess up other checks.
            Table.PushGuard(info);
        }

        public override void ExitQifStatement([NotNull] LuieParser.QifStatementContext context)
        {
            Table.PopGuard();
        }

        public override void EnterForstatement([NotNull] LuieParser.ForstatementContext context)
        {

            string identifier = context.IDENTIFIER().GetText();


            if (Table.IsDefined(identifier))
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
            if (context.identifier?.Text is not string identifier)
            {
                return;
            }

            CheckDefinedness(identifier, context);
            base.ExitFactor(context);
        }

        public override void ExitFunction([NotNull] LuieParser.FunctionContext context)
        {
            FunctionExpression<double> expression = context.GetFunctionExpression<double>();

            expression.UndefinedIdentifiers(Table).ForEach(identifier =>
            {
                Error.Report(new UndefinedError(new ErrorContext(context), identifier));
            });
        }

        public override void EnterGateDeclaration([NotNull] LuieParser.GateDeclarationContext context)
        {
            Table.PushEmtpyScope();
            foreach (Parameter param in context.GetParameters())
            {
                Table.AddSymbol(param);
            }
        }

        public override void ExitGateDeclaration([NotNull] LuieParser.GateDeclarationContext context)
        {
            Table.PopScope();

            // Create emtpy block for declaration analysis
            CodeBlock block = new()
            {
                Parent = null
            };
            CompositeGate gate = new(context.identifier.Text, block, context.GetParameters(), new ErrorContext(context));
            Table.AddSymbol(gate);
        }

        public override void ExitGate([NotNull] LuieParser.GateContext context)
        {
            if (context.type is not null)
            {
                return;
            }

            if (context.parameterizedGate is not null)
            {
                return;
            }

            string? identifier = (context.identifier?.Text) ?? throw new InternalException()
            {
                Reason = $"Gate did have neither a type nor an identifier. ErrorContext = {new ErrorContext(context)}"
            };

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
