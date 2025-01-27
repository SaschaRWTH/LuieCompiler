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
        /// Saves how often a symbol was used.
        /// </summary>
        public readonly Dictionary<Symbol, int> SymbolUsage = [];

        /// <summary>
        /// Indicates whether a register declaration is allowed in the current context.
        /// </summary>
        private bool RegisterDeclarationAllowed => !_gateDeclarationContext;

        /// <summary>
        /// Indicates whether the current context is a gate declaration.
        /// </summary>
        private bool _gateDeclarationContext = false; 

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

            // Check for unused symbols
            foreach (var (symbol, usage) in SymbolUsage)
            {
                if (usage == 0 && symbol.Identifier != "_")
                {
                    Error.Report(new UnusedSymbolWarning(symbol.ErrorContext, symbol));
                }
            }
        }

        public override void EnterBlock([NotNull] LuieParser.BlockContext context)
        {
            Table.PushEmptyScope();
        }

        public override void ExitBlock([NotNull] LuieParser.BlockContext context)
        {
            Table.PopScope();
        }

        public override void ExitRegisterDeclaration([NotNull] LuieParser.RegisterDeclarationContext context)
        {
            ITerminalNode id = context.IDENTIFIER();
            string identifier = id.GetText();

            if (!RegisterDeclarationAllowed)
            {
                Error.Report(new InvalidDeclarationContext(new ErrorContext(context), identifier));
                return;
            }

            if (Table.IsDefined(identifier))
            {
                Error.Report(new RedefineError(new ErrorContext(context.Start), identifier));
            }
            else
            {
                Qubit info = new(identifier, new ErrorContext(context));
                AddSymbolToTable(info);
            }
        }

        public override void EnterGateDeclaration([NotNull] LuieParser.GateDeclarationContext context)
        {
            Table.PushEmptyScope();
            _gateDeclarationContext = true;
            foreach (GateArgument param in context.GetArguments())
            {
                AddSymbolToTable(param);
            }

        }

        public override void ExitGateDeclaration([NotNull] LuieParser.GateDeclarationContext context)
        {
            Table.PopScope();
            _gateDeclarationContext = false;

            // Create empty block for declaration analysis
            CodeBlock block = new()
            {
                Parent = null
            };
            string identifier = context.identifier.Text;
            if (Table.IsDefined(identifier))
            {
                Error.Report(new RedefineError(new ErrorContext(context.Start), identifier));
                return;
            }
            CompositeGate gate = new(identifier, block, context.GetArguments(), new ErrorContext(context));
            AddSymbolToTable(gate);
        }

        public override void ExitConstDeclaration([NotNull] LuieParser.ConstDeclarationContext context)
        {
            string identifier = context.identifier.Text;
            if (Table.IsDefined(identifier))
            {
                Error.Report(new RedefineError(new ErrorContext(context.Start), identifier));
            }
            else
            {
                Symbol info = context.GetConstantSymbol(Table);
                AddSymbolToTable(info);
            }
        }

        public override void EnterRegister([NotNull] LuieParser.RegisterContext context)
        {
            string identifier = context.IDENTIFIER().GetText();
            CheckDefinedness(identifier, context);

            Symbol? info = Table.GetSymbolInfo(identifier);
            if (info is null)
            {
                return;
            }

            if (context.index is not null)
            {
                // Ignore register accesses, can only compare index
                // of access at generation time.
                return;
            }

            if (!Table.SymbolNotGuard(info))
            {
                Error.Report(new UseOfGuardError(new ErrorContext(context), identifier));
            }
        }

        public override void EnterQifStatement([NotNull] LuieParser.QifStatementContext context)
        {
            string identifier = context.register().GetIdentifier();
            Symbol? info = Table.GetSymbolInfo(identifier);

            // Definedness of identifier check in EnterRegister

            // Push potentially null guard such that poping stack does not mess up other checks.
            Table.PushGuard(info);
        }

        public override void ExitQifStatement([NotNull] LuieParser.QifStatementContext context)
        {
            Table.PopGuard();
        }

        public override void EnterForstatement([NotNull] LuieParser.ForstatementContext context)
        {
            Table.PushEmptyScope();

            string identifier = context.IDENTIFIER().GetText();


            if (Table.IsDefined(identifier))
            {
                Error.Report(new RedefineError(new ErrorContext(context.Start), identifier));
                return;
            }

            LuieParser.RangeContext range = context.range();
            LoopIterator loop = range.GetRange(identifier, Table);
            // Dont think these are necessary
            // loop.Start.PropagateSymbolInformation().ForEach(identifier =>
            // {
            //     Compiler.LogError($"The identifier '{identifier}' was not defined in the current context.");
            //     Error.Report(new UndefinedError(new ErrorContext(context), identifier));
            // });
            // loop.End.PropagateSymbolInformation().ForEach(identifier =>
            // {
            //     Compiler.LogError($"The identifier '{identifier}' was not defined in the current context.");
            //     Error.Report(new UndefinedError(new ErrorContext(context), identifier));
            // });
            

            AddSymbolToTable(loop);
        }

        public override void ExitForstatement([NotNull] LuieParser.ForstatementContext context)
        {
            Table.PopScope();
        }

        public override void ExitFactor([NotNull] LuieParser.FactorContext context)
        {
            if (context.identifier?.Text is not string identifier)
            {
                return;
            }

            CheckDefinedness(identifier, context);
        }

        public override void ExitFunction([NotNull] LuieParser.FunctionContext context)
        {
            FunctionExpression<double> expression = context.GetFunctionExpression<double>(Table);

            var undeclared = expression.UndeclaredIdentifiers();
            if (undeclared.Count == 0)
            {
                return;
            }
            
            Compiler.LogError($"The identifiers '{string.Join(',', undeclared)}' were not declared in the current context.");
            Error.Report(new UndefinedError(new ErrorContext(context), undeclared));
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
            Symbol? info = Table.GetSymbolInfo(identifier);
            if (info is not null)
            {
                SymbolUsage[info]++;
                return;
            }

            Compiler.LogError($"The identifier '{identifier}' was not defined in the current context.");
            Error.Report(new UndefinedError(new ErrorContext(context.Start), identifier));
        }

        /// <summary>
        /// Adds a symbol to the symbol table and initializes its usage counter.
        /// </summary>
        /// <param name="symbol"></param>
        private void AddSymbolToTable(Symbol symbol)
        {
            Table.AddSymbol(symbol);
            SymbolUsage[symbol] = 0;
        }
    }

}
