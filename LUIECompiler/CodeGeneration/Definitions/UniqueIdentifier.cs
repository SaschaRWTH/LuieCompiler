using LUIECompiler.Common;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Definitions {
    public class UniqueIdentifier 
    {
        public string Identifier { get; init; }
        public UniqueIdentifier(SymbolTable table) 
        {
            Identifier = table.UniqueIdentifier;
        }
    }
}