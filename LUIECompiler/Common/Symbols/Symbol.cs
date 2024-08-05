using LUIECompiler.Common.Errors;

namespace LUIECompiler.Common.Symbols
{
    /// <summary>
    /// Represents an abstract symbol.
    /// </summary>
    public abstract class Symbol
    {
        /// <summary>
        /// Identifier of the symbol.
        /// </summary>
        public string Identifier { get; init; }

        /// <summary>
        /// Error context of the symbol.
        /// </summary>
        public ErrorContext ErrorContext { get; init; }

        /// <summary>
        /// Creates a new symbol.
        /// </summary>
        /// <param name="identifier">Identifier of the symbol.</param>
        /// <param name="errorContext">Error context of the symbol.</param>
        public Symbol(string identifier, ErrorContext errorContext)
        {
            Identifier = identifier;
            ErrorContext = errorContext;
        }

        override public string ToString()
        {
            return $"Symbol: {{ id={Identifier} }}";
        }
    }

}