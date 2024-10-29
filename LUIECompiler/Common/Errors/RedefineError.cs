
namespace LUIECompiler.Common.Errors
{
    /// <summary>
    /// Represents an error where a register is defined multiple times.
    /// </summary>
    public class RedefineError : IdentifierError
    {
        /// <summary>
        /// Creates a new redefine error.
        /// </summary>
        /// <param name="context">Context where the register was redefined.</param>
        /// <param name="identifier">Identifier that is already defined.</param>
        public RedefineError(ErrorContext context, string identifier) : base(context, identifier)
        {
            Type = ErrorType.Critical;
            Description = $"The register {identifier} is already defined in the context.";
        }
    }

}
