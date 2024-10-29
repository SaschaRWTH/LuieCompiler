namespace LUIECompiler.Common.Errors
{
    /// <summary>
    /// Represents an abstract error where an identifier is evolved.
    /// </summary>
    public abstract class IdentifierError : CompilationError
    {
        /// <summary>
        /// Identifier that caused the error.
        /// </summary>
        public string Identifier { get; }

        public IdentifierError(ErrorContext context, string identifier)
        {
            ErrorContext = context;
            Identifier = identifier;
        }
    }
}