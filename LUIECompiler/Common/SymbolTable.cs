using System.Diagnostics;
using LUIECompiler.Common.Errors;

namespace LUIECompiler.Common
{
    public class SymbolTable
    {

        private int _uniqueId = 0;

        /// <summary>
        /// Gets a unique identifier.
        /// </summary>
        public string UniqueIdenifier
        {
            get
            {
                string id = $"id{_uniqueId}";
                _uniqueId++;
                return id;
            }
        }

        // Needs to be expanded by scope

        /// <summary>
        /// Dictionary that maps the identifier to its symbol information.
        /// </summary>
        public Dictionary<string, SymbolInfo> IdentifierDictionary { get; init; } = new();

        public bool IsDefined(string identifier)
        {
            return IdentifierDictionary.ContainsKey(identifier);
        }

        /// <summary>
        /// Adds a symbol to the table and return a unique identifier for it.
        /// </summary>
        /// <param name="symbolInfo"></param>
        /// <returns></returns>
        public string AddSymbol(SymbolInfo symbolInfo)
        {
            Debug.Assert(!IdentifierDictionary.ContainsKey(symbolInfo.Identifier));
            IdentifierDictionary.Add(symbolInfo.Identifier, symbolInfo);
            return UniqueIdenifier;
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
