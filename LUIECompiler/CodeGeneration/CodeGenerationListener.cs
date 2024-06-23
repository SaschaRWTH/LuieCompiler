
using System.Diagnostics;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using LUIECompiler.CodeGeneration.Statements;
using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration
{
    public class CodeGenerationListener : LuieBaseListener
    {
        public CodeGenerationHandler CodeGen { get; } = new();

        public override void ExitDeclaration([NotNull] LuieParser.DeclarationContext context)
        {
            ITerminalNode id = context.IDENTIFIER();
            string identifier = id.GetText();

            CodeGen.AddRegister(identifier);
        }


        public override void ExitGateapplication([NotNull] LuieParser.GateapplicationContext context)
        {
            string identifier = context.IDENTIFIER().GetText();
            RegisterInfo? info = CodeGen.Table.GetSymbolInfo(identifier) as RegisterInfo 
                                    ?? throw new Exception("identifier has wrong type!");

            Gate gate = new(context);
            GateApplicationStatement statement = new()
            {
                Gate = gate,
                Register = info,
                DefinitionDictionary = CodeGen.DefinitionDictionary,
            };

            CodeGen.AddStatement(statement);
        }

        public override void EnterQifStatement([NotNull] LuieParser.QifStatementContext context)
        {            
            string identifier = context.IDENTIFIER().GetText();
            RegisterInfo? info = CodeGen.Table.GetSymbolInfo(identifier) as RegisterInfo 
                                    ?? throw new Exception("identifier has wrong type!");

            CodeGen.PushGuard(info);
        }

        public override void ExitQifStatement([NotNull] LuieParser.QifStatementContext context)
        {
            CodeGen.PopGuard();
        }

        public override void EnterIfStat([NotNull] LuieParser.IfStatContext context)
        {
            CodeGen.PushCodeBlock();
        }

        public override void ExitIfStat([NotNull] LuieParser.IfStatContext context)
        {
            CodeBlock block = CodeGen.PopCodeBlock();

            QuantumIfStatement statement = new()
            {
                Block = block,
                Guard = CodeGen.CurrentGuard,
                DefinitionDictionary = CodeGen.DefinitionDictionary,
            };
            
            CodeGen.AddStatement(statement);
        }

        public override void EnterElseStat([NotNull] LuieParser.ElseStatContext context)
        {
            CodeGen.PushCodeBlock();
        }

        public override void ExitElseStat([NotNull] LuieParser.ElseStatContext context)
        {            
            CodeBlock block = CodeGen.PopCodeBlock();

            QuantumIfStatement statement = new()
            {
                Block = block,
                Guard = CodeGen.CurrentGuard,
                DefinitionDictionary = CodeGen.DefinitionDictionary,
            };
            
            CodeGen.AddStatement(statement);
        }

    }

}
