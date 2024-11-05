
using Antlr4.Runtime.Misc;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.CodeGeneration.Statements;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Extensions;
using LUIECompiler.Common.Symbols;
using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration
{
    public class CodeGenerationListener : LuieBaseListener
    {
        /// <summary>
        /// Code generations handler for the code generation listerner.
        /// </summary>
        public CodeGenerationHandler CodeGen { get; } = new();

        /// <summary>
        /// Reference to the last poped code block. This is used, e.g., to create if statements.
        /// </summary>
        private Scope? _lastPoped = null;

        public override void ExitRegisterDeclaration([NotNull] LuieParser.RegisterDeclarationContext context)
        {
            Register register = context.GetRegister();
            CodeGen.AddRegister(register, new ErrorContext(context));
        }

        public override void ExitGateapplication([NotNull] LuieParser.GateapplicationContext context)
        {
            List<Symbol> arguments = context.GetArguments(CodeGen.Table);
            IGate gate = context.gate().GetGate(CodeGen.Table);

            if (gate is CompositeGate compositeGate)
            {
                CreateCompositeGate(compositeGate, arguments, new ErrorContext(context));
            }
            else
            {
                CreatePredefinedGate(gate, arguments, new ErrorContext(context));
            }
        }

        /// <summary>
        /// Creates a predefined gate application statement.
        /// </summary>
        /// <param name="gate"></param>
        /// <param name="arguments"></param>
        /// <param name="errorContext"></param>
        /// <exception cref="CodeGenerationException"></exception>
        private void CreatePredefinedGate(IGate gate, List<Symbol> arguments, ErrorContext errorContext)
        {
            if (arguments.Count != gate.NumberOfArguments)
            {
                throw new CodeGenerationException()
                {
                    Error = new InvalidNumberOfArgumentsError(errorContext, gate, arguments.Count),
                };
            }

            GateApplicationStatement statement = new()
            {
                Gate = gate as Gate ?? throw new InternalException()
                {
                    Reason = "Gate was not of type Gate.",
                },
                Arguments = arguments,
                ErrorContext = errorContext,
            };

            CodeGen.AddStatement(statement);
        }

        /// <summary>
        /// Creates a composite gate application statement.
        /// </summary>
        /// <param name="gate"></param>
        /// <param name="arguments"></param>
        private void CreateCompositeGate(CompositeGate gate, List<Symbol> arguments, ErrorContext errorContext)
        {
            CompositeGateStatement statement = new()
            {
                Gate = gate,
                Arguments = arguments.ToDictionary(arg => gate.Arguments[arguments.IndexOf(arg)]),
                ErrorContext = errorContext,
            };

            CodeGen.AddStatement(statement);
        }

        public override void EnterQifStatement([NotNull] LuieParser.QifStatementContext context)
        {
            Symbol? info = context.GetGuard(CodeGen.Table);
            CodeGen.PushGuard(info);
        }

        public override void ExitQifStatement([NotNull] LuieParser.QifStatementContext context)
        {
            CodeGen.PopGuard();
        }


        public override void EnterMainblock([NotNull] LuieParser.MainblockContext context)
        {
            CodeGen.PushMainCodeBlock();
        }

        public override void EnterBlock([NotNull] LuieParser.BlockContext context)
        {
            CodeGen.PushScope();
        }

        public override void ExitBlock([NotNull] LuieParser.BlockContext context)
        {
            _lastPoped = CodeGen.PopCodeBlock();
        }

        public override void ExitIfStat([NotNull] LuieParser.IfStatContext context)
        {
            Scope scope = _lastPoped ?? throw new InternalException()
            {
                Reason = "There was no last poped scope, although block should just have been exited."
            };
            CodeBlock block = scope.CodeBlock ?? throw new InternalException()
            {
                Reason = "Scope did not contain codeblock, although block should existed."
            };

            QuantumIfStatement statement = new()
            {
                Block = block,
                Guard = CodeGen.Table.CurrentGuard,
                ErrorContext = new ErrorContext(context.Start),
            };

            CodeGen.AddStatement(statement);
        }

        public override void ExitElseStat([NotNull] LuieParser.ElseStatContext context)
        {
            Scope scope = _lastPoped ?? throw new InternalException()
            {
                Reason = "There was no last poped scope, although block should just have been exited."
            };
            CodeBlock block = scope.CodeBlock ?? throw new InternalException()
            {
                Reason = "Scope did not contain codeblock, although block should existed."
            };

            QuantumIfStatement statement = new()
            {
                Block = block,
                Guard = CodeGen.Table.CurrentGuard,
                ErrorContext = new ErrorContext(context.Start),
            };

            CodeGen.AddStatement(statement);
        }

        public override void EnterForstatement([NotNull] LuieParser.ForstatementContext context)
        {
            LoopIterator iterator = context.GetIterator();
            CodeGen.AddIterator(iterator, new ErrorContext(context.Start));
        }

        public override void ExitForstatement([NotNull] LuieParser.ForstatementContext context)
        {
            Scope scope = _lastPoped ?? throw new InternalException()
            {
                Reason = "There was no last poped scope, although block should just have been exited."
            };
            CodeBlock block = scope.CodeBlock ?? throw new InternalException()
            {
                Reason = "Scope did not contain codeblock, although block should existed."
            };

            string identifier = context.IDENTIFIER().GetText(); ;
            LoopIterator iterator = CodeGen.Table.GetSymbolInfo(identifier) as LoopIterator ?? throw new InternalException()
            {
                Reason = $"Iterator {identifier} was not found in the symbol table or of wrong type.",
            };

            ForLoopStatement statement = new()
            {
                Iterator = iterator,
                Body = block,
                ErrorContext = new ErrorContext(context.Start),
            };

            CodeGen.AddStatement(statement);
        }

        public override void EnterGateDeclaration([NotNull] LuieParser.GateDeclarationContext context)
        {
            CodeGen.Table.PushEmptyScope();
            foreach (GateArgument arg in context.GetArguments())
            {
                CodeGen.AddArgument(arg, new(context));
            }
        }

        public override void ExitGateDeclaration([NotNull] LuieParser.GateDeclarationContext context)
        {
            List<GateArgument> arguments = CodeGen.Table.GetArguments();
            CodeGen.Table.PopScope();

            Scope scope = _lastPoped ?? throw new InternalException()
            {
                Reason = "There was no last poped scope, although block should just have been exited."
            };
            CodeBlock block = scope.CodeBlock ?? throw new InternalException()
            {
                Reason = "Scope did not contain codeblock, although block should existed."
            };
            CompositeGate gate = new(context.identifier.Text, block, arguments, new ErrorContext(context));
            CodeGen.AddCompositeGate(gate, new(context));
        }

        public override void ExitConstDeclaration([NotNull] LuieParser.ConstDeclarationContext context)
        {
            Symbol symbol = context.GetConstantSymbol();
            CodeGen.AddConstant(symbol, new ErrorContext(context));
        }

    }

}
