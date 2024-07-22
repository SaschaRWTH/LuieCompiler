
using Antlr4.Runtime.Misc;
using LUIECompiler.CodeGeneration.Gates;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.CodeGeneration.Expressions;
using LUIECompiler.CodeGeneration.Statements;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Extensions;
using LUIECompiler.Common.Symbols;

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
        private CodeBlock? _lastPoped = null;

        public override void ExitRegisterDeclaration([NotNull] LuieParser.RegisterDeclarationContext context)
        {
            Register register = context.GetRegister();
            CodeGen.AddRegister(register, new ErrorContext(context.Start));
        }

        public override void ExitGateapplication([NotNull] LuieParser.GateapplicationContext context)
        {
            List<Qubit> parameters = context.GetParameters(CodeGen.Table);
            Gate gate = context.gate().GetGate(CodeGen.Table);
            if (parameters.Count != gate.NumberOfArguments)
            {
                throw new CodeGenerationException()
                {
                    Error = new InvalidArguments(new ErrorContext(context.Start), gate, parameters.Count),
                };
            }

            GateApplicationStatement statement = new()
            {
                Gate = gate,
                Parameters = parameters,
                ParentBlock = CodeGen.CurrentBlock,
                ErrorContext = new ErrorContext(context.Start),
            };

            CodeGen.AddStatement(statement);
        }

        public override void EnterQifStatement([NotNull] LuieParser.QifStatementContext context)
        {
            Qubit? info = context.GetGuard(CodeGen.Table);
            CodeGen.PushGuard(info);
        }

        public override void ExitQifStatement([NotNull] LuieParser.QifStatementContext context)
        {
            CodeGen.PopGuard();
        }

        public override void EnterBlock([NotNull] LuieParser.BlockContext context)
        {
            CodeGen.PushCodeBlock();
        }

        public override void ExitBlock([NotNull] LuieParser.BlockContext context)
        {
            _lastPoped = CodeGen.PopCodeBlock();
        }

        public override void ExitIfStat([NotNull] LuieParser.IfStatContext context)
        {
            CodeBlock block = _lastPoped ?? throw new InternalException()
            {
                Reason = "There was no last poped code block, although block should just have been exited."
            };

            QuantumIfStatement statement = new()
            {
                Block = block,
                Guard = CodeGen.CurrentGuard,
                ParentBlock = CodeGen.CurrentBlock,
                ErrorContext = new ErrorContext(context.Start),
            };

            CodeGen.AddStatement(statement);
        }

        public override void ExitElseStat([NotNull] LuieParser.ElseStatContext context)
        {
            CodeBlock block = _lastPoped ?? throw new InternalException()
            {
                Reason = "There was no last poped code block, although block should just have been exited."
            };

            QuantumIfStatement statement = new()
            {
                Block = block,
                Guard = CodeGen.CurrentGuard,
                ParentBlock = CodeGen.CurrentBlock,
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
            CodeBlock block = _lastPoped ?? throw new InternalException()
            {
                Reason = "There was no last poped code block, although block should just have been exited."
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
                ParentBlock = CodeGen.CurrentBlock,
                ErrorContext = new ErrorContext(context.Start),
            };

            CodeGen.AddStatement(statement);
        }

    }

}
