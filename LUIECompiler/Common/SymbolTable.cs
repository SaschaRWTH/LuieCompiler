using System.Diagnostics;

namespace LUIECompiler.Common
{
    public class SymbolTable
    {
        // Needs to be expanded by scope

        public Dictionary<string, SymbolInfo> identifierDictionary { get; init; } = new();

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
        public RegisterInfo(string identifier) : base(identifier) { }
    }

    public class SymbolInfo
    {
        public string Identifier { get; init; }

        public SymbolInfo(string identifier)
        {
            Identifier = identifier;
        }
    }

}
