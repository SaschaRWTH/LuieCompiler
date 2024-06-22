
using Antlr4.Runtime.Misc;
using LUIECompiler.CodeGeneration.Statements;
using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration
{
    public class CodeGenerationListener : LuieBaseListener
    {
        public CodeGenerationHandler CodeGen { get; } = new();

        public override void ExitGateapplication([NotNull] LuieParser.GateapplicationContext context)
        {
            Gate gate = new(context);
            string identifier = context.IDENTIFIER().GetText();
            
            // TODO: Improve error handling
            RegisterInfo? info = CodeGen.Table.GetSymbolInfo(identifier) as RegisterInfo ?? throw new Exception("identifier has wrong type!");

            GateApplicationStatement statement = new()
            {
                Gate = gate,
                Register = info,
                DefinitionDictionary = CodeGen.DefinitionDictionary,
            };

            CodeGen.AddStatement(statement);
        }

        public override void EnterIfStat([NotNull] LuieParser.IfStatContext context)
        {
            CodeGen.PushCodeBlock();
        }

        public override void ExitIfStat([NotNull] LuieParser.IfStatContext context)
        {
            CodeBlock block = CodeGen.PopCodeBlock();
            
            string identifier = context.IDENTIFIER().GetText();
            
            // TODO: Improve error handling
            RegisterInfo? info = CodeGen.Table.GetSymbolInfo(identifier) as RegisterInfo ?? throw new Exception("identifier has wrong type!");

            QuantumIfStatement statement = new()
            {
                Block = block,
                Guard = info,
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
            
            // TODO: add guard stack or similar, could streamline grammar for both to use guard stack e.g. 
            // qifStatement: IF IDENTIFIER ifStat elseStat? END 
            // ifState : DO block
            string identifier = "";
            // string identifier = context.IDENTIFIER().GetText();
            
            // TODO: Improve error handling
            RegisterInfo? info = CodeGen.Table.GetSymbolInfo(identifier) as RegisterInfo ?? throw new Exception("identifier has wrong type!");

            QuantumIfStatement statement = new()
            {
                Block = block,
                Guard = info,
                DefinitionDictionary = CodeGen.DefinitionDictionary,
            };
            
            CodeGen.AddStatement(statement);
        }

    }

}
