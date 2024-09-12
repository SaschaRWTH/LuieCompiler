using LUIECompiler.Common.Symbols;

namespace LUIECompiler.Common.Errors
{
    public class UnusedSymbolWarning : CompilationError
    {
        /// <summary>
        /// Symbol that was never used.
        /// </summary>
        public Symbol Symbol { get; }

        /// <summary>
        /// Creates a new invalid range warning.
        /// </summary>
        /// <param name="context">Context where the range was created.</param>
        /// <param name="start">Start value of the range.</param>
        /// <param name="end">End value of the range.</param>
        public UnusedSymbolWarning(ErrorContext context, Symbol symbol)
        {
            Type = ErrorType.Warning;
            ErrorContext = context;
            Symbol = symbol;
            Description = $"The {Symbol.GetType()} '{Symbol.Identifier}' was defined but never used.";
        }

    }
}