using System.Diagnostics;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.Common
{
    public class SymbolTable
    {
        /// <summary>
        /// A unique id for each identifier in the symbol table.
        /// </summary>
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

        /// <summary>
        /// Stack of scopes mapping an identifier to the corrisponding <see cref="Symbol"/>. 
        /// </summary>
        public Stack<Dictionary<string, Symbol>> ScopeStack { get; init; } = new();

        /// <summary>
        /// Dictionary that maps the identifier to its symbol information.
        /// </summary>
        public Dictionary<string, Symbol> CurrentIdentifierDictionary
        {
            get
            {
                if (ScopeStack.Count == 0)
                {
                    throw new InternalException() { Reason = "Tried peeking an empty scope stack." };
                }
                return ScopeStack.Peek() ?? throw new InternalException() { Reason = "Top most element on scope stack null." };
            }
        }

        /// <summary>
        /// Checks whether a given <paramref name="identifier"/> is definined.
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public bool IsDefined(string identifier)
        {
            foreach (var dict in ScopeStack)
            {
                if (dict.ContainsKey(identifier))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks whether a given <paramref name="identifier"/> is definined in the current scope.
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public bool IsDefinedInCurrentScop(string identifier)
        {
            return CurrentIdentifierDictionary.ContainsKey(identifier);
        }

        /// <summary>
        /// Adds a symbol to the table and return a unique identifier for it.
        /// </summary>
        /// <param name="symbolInfo"></param>
        /// <returns></returns>
        public string AddSymbol(Symbol symbolInfo)
        {
            Debug.Assert(!IsDefinedInCurrentScop(symbolInfo.Identifier));
            CurrentIdentifierDictionary.Add(symbolInfo.Identifier, symbolInfo);
            return UniqueIdenifier;
        }

        /// <summary>
        /// Pushes a new scope onto the scope stack.
        /// </summary>
        public void PushScope()
        {
            ScopeStack.Push([]);
        }


        /// <summary>
        /// Pops the current scope.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Symbol> PopScope()
        {
            return ScopeStack.Pop();
        }

        /// <summary>
        /// Gets the 
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public Symbol? GetSymbolInfo(string identifier)
        {
            foreach (var dict in ScopeStack)
            {
                if (dict.TryGetValue(identifier, out var info))
                {
                    return info;
                }
            }
            return null;
        }
    }

}
