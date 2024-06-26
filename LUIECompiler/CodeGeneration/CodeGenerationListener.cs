
using System.Diagnostics;
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
        public CodeGenerationHandler CodeGen { get; } = new();

        public override void ExitDeclaration([NotNull] LuieParser.DeclarationContext context)
        {
            ITerminalNode id = context.IDENTIFIER();
            string identifier = id.GetText();

            CodeGen.AddRegister(identifier, context.Start.Line);
        }


        public override void ExitGateapplication([NotNull] LuieParser.GateapplicationContext context)
        {
            string identifier = context.IDENTIFIER().GetText();
            Register? info = CodeGen.GetSymbolInfo(identifier, context.Start.Line) as Register
                                    ?? throw new CodeGenerationException()
                                    {
                                        Error = new TypeError(context.Start.Line, identifier),
                                    };

            Gate gate = new(context);
            int line = context.Start.Line;
            GateApplicationStatement statement = new()
            {
                Gate = gate,
                Register = info,
                DefinitionDictionary = CodeGen.DefinitionDictionary,
                Line = line,
            };

            CodeGen.AddStatement(statement);
        }

        public override void EnterQifStatement([NotNull] LuieParser.QifStatementContext context)
        {
            string identifier = context.IDENTIFIER().GetText();
            Register? info = CodeGen.GetSymbolInfo(identifier, context.Start.Line) as Register
                                    ?? throw new CodeGenerationException()
                                    {
                                        Error = new TypeError(context.Start.Line, identifier),
                                    };

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
            CodeGen.PopCodeBlock();
        }

        public override void ExitIfStat([NotNull] LuieParser.IfStatContext context)
        {
            CodeBlock block = CodeGen.CurrentBlock;

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
            CodeBlock block = CodeGen.CurrentBlock;

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
