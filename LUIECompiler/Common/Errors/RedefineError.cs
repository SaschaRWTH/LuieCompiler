
namespace LUIECompiler.Common.Errors
{
    /// <summary>
    /// Represents an error where a register is defined multiple times.
    /// </summary>
    public class RedefineError : CompilationError
    {
        /// <summary>
        /// Identifier that is already defined.
        /// </summary>
        public string Identifier { get; init; }

        /// <summary>
        /// Creates a new redefine error.
        /// </summary>
        /// <param name="context">Context where the register was redefined.</param>
        /// <param name="identifier">Identifier that is already defined.</param>
        public RedefineError(ErrorContext context, string identifier)
        {
            Type = ErrorType.Critical;
            ErrorContext = context;
            Identifier = identifier;
            Description = $"The register {identifier} is already defined in the context.";
        }
    }

}
