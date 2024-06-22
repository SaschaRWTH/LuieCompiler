
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

    }

}
