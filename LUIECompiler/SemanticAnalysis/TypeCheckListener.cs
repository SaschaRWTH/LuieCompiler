using Antlr4.Runtime.Misc;
using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Gates;
using LUIECompiler.CodeGeneration.Expressions;
using LUIECompiler.Common;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Extensions;
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

        public override void ExitRegisterDeclaration([NotNull] LuieParser.RegisterDeclarationContext context)
        {
            Register reg = context.GetRegister();
            Table.AddSymbol(reg);
        }

        public override void ExitRegister([NotNull] LuieParser.RegisterContext context)
        {
            string identifier = context.IDENTIFIER().GetText();

            Symbol? symbol = Table.GetSymbolInfo(identifier);
            if (symbol == null)
            {
                Error.Report(new UndefinedError(new ErrorContext(context.Start), identifier));
                return;
            }

            // Check type of parameter at generation time
            if (symbol is Parameter)
            {
                return;
            }

            // Cannot access qubit with []
            if (symbol is Qubit && context.TryGetIndexExpression(out Expression<int> _))
            {
                Error.Report(new UndefinedError(new ErrorContext(context.Start), identifier));
            }
        }

        public override void ExitFactor([NotNull] LuieParser.FactorContext context)
        {
            // Only allow factors in register to be iterator (for now)
            string? identifier = context.identifier?.Text;
            if (identifier == null)
            {
                return;
            }

            Symbol? symbol = Table.GetSymbolInfo(identifier);
            if (symbol == null)
            {
                Error.Report(new UndefinedError(new ErrorContext(context.Start), identifier));
                return;
            }

            if (symbol is not LoopIterator)
            {
                Error.Report(new TypeError(new ErrorContext(context.Start), identifier, typeof(LoopIterator), symbol.GetType()));
            }
        }

        public override void ExitGateapplication([NotNull] LuieParser.GateapplicationContext context)
        {
            List<LuieParser.RegisterContext> registers = context.register().ToList();

            foreach (var register in registers)
            {
                string identifier = register.GetIdentifier();
                Symbol? symbol = Table.GetSymbolInfo(identifier);
                if (symbol == null)
                {
                    Error.Report(new UndefinedError(new ErrorContext(context.Start), identifier));
                    return;
                }


                // Check type of parameter at generation time
                if (symbol is Parameter)
                {
                    return;
                }

                if (symbol is not Register)
                {
                    Error.Report(new TypeError(new ErrorContext(context.Start), identifier, typeof(Register), symbol.GetType()));
                }

                if (symbol is not Qubit && !register.IsRegisterAccess())
                {
                    // Returning typeof(Qubit) is not perfect, technically RegisterAccess is of type Qubit, but the user could still be confused. 
                    Error.Report(new TypeError(new ErrorContext(context.Start), identifier, typeof(Qubit), symbol.GetType()));
                }
            }

            Gate gate = context.gate().GetGate(Table);

            if (gate.NumberOfArguments != registers.Count)
            {
                Error.Report(new InvalidArguments(new ErrorContext(context.Start), gate, registers.Count));
            }

        }

        public override void ExitQifStatement([NotNull] LuieParser.QifStatementContext context)
        {
            LuieParser.RegisterContext registerContext = context.register();
            string identifier = registerContext.GetIdentifier();

            Symbol? symbol = Table.GetSymbolInfo(identifier);
            if (symbol == null)
            {
                Error.Report(new UndefinedError(new ErrorContext(context.Start), identifier));
                return;
            }

            // Check type of parameter at generation time
            if (symbol is Parameter)
            {
                return;
            }

            if (symbol is Qubit)
            {
                return;
            }

            if (symbol is Register && registerContext.TryGetIndexExpression(out Expression<int> _))
            {
                return;
            }

            Error.Report(new TypeError(new ErrorContext(context.Start), identifier, typeof(Qubit), symbol.GetType()));
        }

        public override void EnterForstatement([NotNull] LuieParser.ForstatementContext context)
        {
            string identifier = context.IDENTIFIER().GetText();
            LoopIterator loopIterator = context.range().GetRange(identifier);

            Table.AddSymbol(loopIterator);
        }

        public override void ExitRange([NotNull] LuieParser.RangeContext context)
        {
            if (!int.TryParse(context.start.Text, out int start) || !int.TryParse(context.end.Text, out int end))
            {
                return;
            }

            if (start >= end)
            {
                Error.Report(new InvalidRangeWarning(new ErrorContext(context), start, end));
            }
        }

        public override void ExitGate([NotNull] LuieParser.GateContext context)
        {
            if (context.type is not null)
            {
                return;
            }

            if (context.identifier?.Text is string identifier)
            {
                Symbol? symbol = Table.GetSymbolInfo(identifier);
                if (symbol == null)
                {
                    Error.Report(new UndefinedError(new ErrorContext(context.Start), identifier));
                    return;
                }

                if (symbol is not CompositeGate)
                {
                    Error.Report(new TypeError(new ErrorContext(context.Start), identifier, typeof(CompositeGate), symbol.GetType()));
                }
            }
        }

        public override void ExitFunction([NotNull] LuieParser.FunctionContext context)
        {
            string identifier = context.param.Text;

            Symbol? symbol = Table.GetSymbolInfo(identifier);
            if (symbol == null)
            {
                Error.Report(new UndefinedError(new ErrorContext(context), identifier));
                return;
            }

            // Check type of parameter at generation time
            if (symbol is Parameter)
            {
                return;
            }

            if (symbol is not Register)
            {
                Error.Report(new TypeError(new ErrorContext(context), identifier, typeof(Register), symbol.GetType()));
            }
        }

        
        public override void EnterGateDeclaration([NotNull] LuieParser.GateDeclarationContext context)
        {
            Table.PushScope();
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
            CompositeGate gate = new(context.identifier.Text, block, new ErrorContext(context));
            Table.AddSymbol(gate);
        }
    }

}
