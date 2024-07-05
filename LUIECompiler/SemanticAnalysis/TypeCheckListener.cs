using System.Data.Common;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.CodeGeneration.Exceptions;
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
            List<LuieParser.RegisterContext> registers = context.register().ToList();

            foreach(var register in registers)
            {
                string identifier = register.GetIdentifier();
                Symbol? symbol = Table.GetSymbolInfo(identifier);
                if (symbol == null)
                {
                    Error.Report(new UndefinedError(context.Start.Line, identifier));
                    return;
                }

                if (symbol is not Register)
                {
                    Error.Report(new TypeError(context.Start.Line, identifier, typeof(Register), symbol.GetType()));
                }

                if (symbol is not Qubit && !register.TryGetIndex(out int _))
                {
                    // Returning typeof(Qubit) is not perfect, technically RegisterAccess is of type Qubit, but the user could still be confused. 
                    Error.Report(new TypeError(context.Start.Line, identifier, typeof(Qubit), symbol.GetType()));
                }
            }

            Gate gate = new(context);

            if(gate.NumberOfArguments != registers.Count)
            {
                Error.Report(new InvalidArguments(context.Start.Line, gate, registers.Count));
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

            Error.Report(new TypeError(context.Start.Line, identifier, typeof(Qubit), symbol.GetType()));
        }

        public override void EnterForstatement([NotNull] LuieParser.ForstatementContext context)
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

            LoopIterator loopIterator = new(identifier, start, end);
            Table.AddSymbol(loopIterator);
        }
    }

}
