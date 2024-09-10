using System.Diagnostics;
using LUIECompiler.CodeGeneration;
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
        public string UniqueIdentifier
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
        public Stack<Scope> ScopeStack { get; init; } = new();

        /// <summary>
        /// Gets the current scope from the scope stack.
        /// </summary>
        public Scope CurrentScope
        {
            get
            {
                return ScopeStack.Peek() ?? throw new InternalException()
                {
                    Reason = "Tried peeking an empty scope stack."
                };
            }
        }

        /// <summary>
        /// Dictionary that maps the identifier to its symbol information.
        /// </summary>
        public Dictionary<string, Symbol> CurrentIdentifierDictionary
        {
            get
            {
                if (ScopeStack.Count == 0)
                {
                    throw new InternalException()
                    {
                        Reason = "Tried peeking an empty scope stack."
                    };
                }

                return CurrentScope.IdentifierMap;
            }
        }

        /// <summary>
        /// Stack of the registers guarding the if-clauses.
        /// </summary>
        public Stack<Symbol> GuardStack { get; } = [];

        /// <summary>
        /// Gets guard of the current if statement.
        /// </summary>
        public Symbol CurrentGuard
        {
            get => GuardStack.Peek() ?? throw new InternalException()
            {
                Reason = "Tried to peek empty guard stack.",
            };
        }

        /// <summary>
        /// Checks whether a given <paramref name="identifier"/> is definined.
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public bool IsDefined(string identifier)
        {
            foreach (var dict in ScopeStack.Select(scope => scope.IdentifierMap))
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
        public void AddSymbol(Symbol symbolInfo)
        {
            Debug.Assert(!IsDefinedInCurrentScop(symbolInfo.Identifier));
            CurrentIdentifierDictionary.Add(symbolInfo.Identifier, symbolInfo);
        }

        /// <summary>
        /// Pushes scope with an emtpy code block onto the scope stack.
        /// </summary>
        public void PushEmtpyScope()
        {
            ScopeStack.Push(new Scope()
            {
                CodeBlock = new CodeBlock()
                {
                    Parent = CurrentScope.CodeBlock,
                }
            });
        }

        /// <summary>
        /// Pushes a new scope onto the scope stack.
        /// </summary>
        public void PushScope(CodeBlock codeBlock)
        {
            ScopeStack.Push(new Scope()
            {
                CodeBlock = codeBlock,
            });
        }


        /// <summary>
        /// Pops the current scope.
        /// </summary>
        /// <returns></returns>
        public Scope PopScope()
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
            foreach (var dict in ScopeStack.Select(scope => scope.IdentifierMap))
            {
                if (dict.TryGetValue(identifier, out var info))
                {
                    return info;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets all parameters in the symbol table.
        /// </summary>
        /// <returns></returns>
        public List<Parameter> GetParameters()
        {
            List<Parameter> parameters = [];
            foreach (var scope in ScopeStack)
            {
                parameters.AddRange(scope.GetParameters());
            }
            return parameters;
        }
    }

}
