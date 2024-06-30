
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.CodeGeneration.Statements;
using LUIECompiler.Common;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration
{
    public class CodeGenerationListener : LuieBaseListener
    {
        /// <summary>
        /// Code generations handler for the code generation listerner.
        /// </summary>
        public CodeGenerationHandler CodeGen { get; } = new();

        private CodeBlock? _lastPoped = null;

        public override void ExitDeclaration([NotNull] LuieParser.DeclarationContext context)
        {
            string identifier = context.IDENTIFIER().GetText();

            if (context.TryGetSize(out int size))
            {
                CodeGen.AddRegister(identifier, size, context.Start.Line);
            }
            else
            {
                CodeGen.AddQubit(identifier, context.Start.Line);
            }

        }


        public override void ExitGateapplication([NotNull] LuieParser.GateapplicationContext context)
        {
            List<Qubit> paramerts = context.GetParameter(CodeGen.Table);

            Gate gate = new(context);
            int line = context.Start.Line;
            GateApplicationStatement statement = new()
            {
                Gate = gate,
                // TODO: Change to params
                Qubit = paramerts[0],
                DefinitionDictionary = CodeGen.DefinitionDictionary,
                Line = line,
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

            int line = context.Start.Line;
            QuantumIfStatement statement = new()
            {
                Block = block,
                Guard = CodeGen.CurrentGuard,
                DefinitionDictionary = CodeGen.DefinitionDictionary,
                Line = line,
            };

            CodeGen.AddStatement(statement);
        }

        public override void ExitElseStat([NotNull] LuieParser.ElseStatContext context)
        {
            CodeBlock block = _lastPoped ?? throw new InternalException()
            {
                Reason = "There was no last poped code block, although block should just have been exited."
            };

            int line = context.Start.Line;
            QuantumIfStatement statement = new()
            {
                Block = block,
                Guard = CodeGen.CurrentGuard,
                DefinitionDictionary = CodeGen.DefinitionDictionary,
                Line = line,
            };

            CodeGen.AddStatement(statement);
        }

    }

}
