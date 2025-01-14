namespace LUIECompiler.Common.Errors
{
    /// <summary>
    /// Represents an error where an identifier may not be declared the given context.
    /// </summary>
    public class InvalidDeclarationContext : IdentifierError
    {

        /// <summary>
        /// Creates a new invalid declaration context error.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="identifier"></param>
        public InvalidDeclarationContext(ErrorContext context, string identifier) : base(context, identifier)
        {
            Type = ErrorType.Critical;
            ErrorContext = context;
            Description = $"The identifier {identifier} may not be declared in the current context.";
        }
    }
}