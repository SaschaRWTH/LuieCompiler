using System.Data.Common;
using System.Diagnostics;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace LUIECompiler.SemanticAnalysis
{
    public class DeclarationAnalysisListener : LuieBaseListener
    {
        private readonly SymbolTable table = new();

        public override void EnterBlock([NotNull] LuieParser.BlockContext context)
        {
            
            base.EnterBlock(context);
        }

        public override void ExitBlock([NotNull] LuieParser.BlockContext context)
        {
            base.ExitBlock(context);
        }

        public override void ExitDeclaration([NotNull] LuieParser.DeclarationContext context)
        {
            ITerminalNode id = context.IDENTIFIER();
            string identifier = id.GetText();
            if(table.IsDefined(identifier))
            {
                Console.WriteLine($"Error! Identifier '{identifier}' was already declared.");
                // Add error
            }
            else
            {
                RegisterInfo info = new(identifier);
                table.AddSymbol(info);
            }

            base.ExitDeclaration(context);
        }

        public override void ExitStatement([NotNull] LuieParser.StatementContext context)
        {
            base.ExitStatement(context);
        }

        public override void ExitGateapplication([NotNull] LuieParser.GateapplicationContext context)
        {
            var node = context.IDENTIFIER();
            string identifier = node.GetText();
            if(!table.IsDefined(identifier))
            {
                Console.WriteLine($"Error! Identifier '{identifier}' was not declared");
            }
        }

        public override void ExitIfStat([NotNull] LuieParser.IfStatContext context)
        {
            var node = context.IDENTIFIER();
            string identifier = node.GetText();
            if(!table.IsDefined(identifier))
            {
                Console.WriteLine($"Error! Identifier '{identifier}' was not declared");
            }
        }

    }

    public class SymbolTable 
    {
        // Needs to be expanded by scope

        public readonly Dictionary<string, SymbolInfo> identifierDictionary = new();

        public bool IsDefined(string identifier)
        {
            return identifierDictionary.ContainsKey(identifier);
        }

        public void AddSymbol(SymbolInfo symbolInfo)
        {
            Debug.Assert(!identifierDictionary.ContainsKey(symbolInfo.Identifier));
            identifierDictionary.Add(symbolInfo.Identifier, symbolInfo);
        }
    }

    public class RegisterInfo : SymbolInfo
    {
        public RegisterInfo(string identifier) : base(identifier) {}
    }

    public class SymbolInfo 
    {
        public readonly string Identifier;

        public SymbolInfo(string identifier)
        {
            Identifier = identifier;
        } 
    }

}
