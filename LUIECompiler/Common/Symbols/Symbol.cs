using LUIECompiler.Common.Errors;

namespace LUIECompiler.Common.Symbols
{

    public abstract class Symbol
    {
        /// <summary>
        /// Identifier of the symbol.
        /// </summary>
        public string Identifier { get; init; }

        public ErrorContext ErrorContext { get; init; }

        public Symbol(string identifier, ErrorContext errorContext)
        {
            Identifier = identifier;
            ErrorContext = errorContext;
        }
    }

}