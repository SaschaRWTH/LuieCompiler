
namespace LUIECompiler.Common.Errors
{
    /// <summary>
    /// Represents an error where an identifier is undefined.
    /// </summary>
    public class UndefinedError : CompilationError
    {
        /// <summary>
        /// Identifiers that are undefined.
        /// </summary>
        public List<string> Identifier { get; init; }

        /// <summary>
        /// Creates a new undefined error.
        /// </summary>
        /// <param name="context">Context where the identifier was used.</param>
        /// <param name="identifier">Identifier that is undefined.</param>
        public UndefinedError(ErrorContext context, string identifier)
        {
            Type = ErrorType.Critical;
            ErrorContext = context;
            Identifier = [identifier];
            Description = $"The identifier {identifier} does not exist in the context.";
        }

        /// <summary>
        /// Creates a new undefined error.
        /// </summary>
        /// <param name="context">Context where the identifiers were used.</param>
        /// <param name="identifiers">Identifiers that are undefined.</param>
        public UndefinedError(ErrorContext context, List<string> identifiers)
        {
            Type = ErrorType.Critical;
            ErrorContext = context;
            Identifier = [.. identifiers];
            Description = $"The identifiers {string.Join(", ", identifiers)} do not exist in the context.";
        }
    }

}
