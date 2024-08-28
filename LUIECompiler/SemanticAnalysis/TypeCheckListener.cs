using Antlr4.Runtime.Misc;
using LUIECompiler.CodeGeneration;
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

        
        public override void EnterMainblock([NotNull] LuieParser.MainblockContext context)
        {
            Table.PushScope();
        }

        public override void ExitMainblock([NotNull] LuieParser.MainblockContext context)
        {
            // Technically not needed, just for completeness.
            Table.PopScope();
        }

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
                Compiler.PrintLog($"TypeCheckListener.ExitRegister: Could not get the symbol of identifier '{identifier}' from the symbol table.");
                Error.Report(new UndefinedError(new ErrorContext(context), identifier));
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
                Compiler.PrintLog($"TypeCheckListener.ExitRegister: The symbol '{identifier}' was neither a qubit nor a accessed register.");
                Error.Report(new UndefinedError(new ErrorContext(context), identifier));
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
                Compiler.PrintLog($"TypeCheckListener.ExitFactor: Could not get the symbol of identifier '{identifier}' from the symbol table.");
                Error.Report(new UndefinedError(new ErrorContext(context), identifier));
                return;
            }

            if (symbol is not LoopIterator)
            {
                Compiler.PrintLog($"The symbol '{identifier}' was not a LoopIterator.");
                Error.Report(new TypeError(new ErrorContext(context), identifier, typeof(LoopIterator), symbol.GetType()));
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
                    Compiler.PrintLog($"TypeCheckListener.ExitGateapplication: Could not get the symbol of identifier '{identifier}'. Symbol is not a register.");
                    Error.Report(new TypeError(new ErrorContext(context), identifier, typeof(Register), symbol.GetType()));
                }

                if (symbol is not Qubit && !register.IsRegisterAccess())
                {
                    Compiler.PrintLog($"TypeCheckListener.ExitGateapplication: Could not get the symbol of identifier '{identifier}'. Symbol is neither a qubit nor accessed.");
                    // Returning typeof(Qubit) is not perfect, technically RegisterAccess is of type Qubit, but the user could still be confused. 
                    Error.Report(new TypeError(new ErrorContext(context), identifier, typeof(Qubit), symbol.GetType()));
                }
            }

            IGate gate = context.gate().GetGate(Table);

            if (gate.NumberOfArguments != registers.Count)
            {
                Compiler.PrintLog($"The number of arguments are invalid for the used gate.");
                Error.Report(new InvalidArguments(new ErrorContext(context), gate, registers.Count));
            }

        }

        public override void ExitQifStatement([NotNull] LuieParser.QifStatementContext context)
        {
            LuieParser.RegisterContext registerContext = context.register();
            string identifier = registerContext.GetIdentifier();

            Symbol? symbol = Table.GetSymbolInfo(identifier);
            if (symbol == null)
            {
                Compiler.PrintLog($"Could not get the symbol of identifier '{identifier}' from the symbol table.");
                Error.Report(new UndefinedError(new ErrorContext(context), identifier));
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

            Compiler.PrintLog($"The symbol {identifier} is neither a qubit nor an accessed register.");
            Error.Report(new TypeError(new ErrorContext(context), identifier, typeof(Qubit), symbol.GetType()));
        }

        public override void EnterForstatement([NotNull] LuieParser.ForstatementContext context)
        {
            string identifier = context.IDENTIFIER().GetText();
            LoopIterator loopIterator = context.range().GetRange(identifier);

            Table.AddSymbol(loopIterator);
        }

        public override void ExitRange([NotNull] LuieParser.RangeContext context)
        {
            if (!int.TryParse(context.start?.Text, out int start) || !int.TryParse(context.end?.Text, out int end))
            {
                return;
            }

            if (start >= end)
            {
                Compiler.PrintLog($"The start of the range is smaller of equal to the end index of the range.");
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
                if (symbol is null)
                {
                    Compiler.PrintLog($"The gate identifier '{identifier}' could not be found in the symbol table.");
                    Error.Report(new UndefinedError(new ErrorContext(context), identifier));
                    return;
                }

                if (symbol is not CompositeGate)
                {
                    Compiler.PrintLog($"The symbol '{identifier}' is neither a known gate nor a composite gate.");
                    Error.Report(new TypeError(new ErrorContext(context), identifier, typeof(CompositeGate), symbol.GetType()));
                }
            }
        }

        public override void ExitFunction([NotNull] LuieParser.FunctionContext context)
        {
            FunctionExpression<double> expression = context.GetFunctionExpression<double>();

            expression.UndefinedIdentifiers(Table).ForEach(identifier =>
            {
                Error.Report(new UndefinedError(new ErrorContext(context), identifier));
            });

            if(expression is SizeOfFunctionExpression<double> sizeOfFunctionExpression)
            {
                foreach(string identifier in sizeOfFunctionExpression.Parameter)
                {
                    Symbol? symbol = Table.GetSymbolInfo(identifier);
                    if (symbol is null)
                    {
                        Compiler.PrintLog($"Could not get the symbol of identifier '{identifier}' from the symbol table.");
                        Error.Report(new UndefinedError(new ErrorContext(context), identifier));
                        continue;
                    }

                    // Check type of parameter at generation time
                    if (symbol is Parameter)
                    {
                        continue;
                    }

                    if (symbol is not Register)
                    {
                        Compiler.PrintLog($"Could not get the symbol of identifier '{identifier}' from the symbol table.");
                        Error.Report(new TypeError(new ErrorContext(context), identifier, typeof(Register), symbol.GetType()));
                    }
                }
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
            CompositeGate gate = new(context.identifier.Text, block, context.GetParameters(), new ErrorContext(context));
            Table.AddSymbol(gate);
        }
    }

}
