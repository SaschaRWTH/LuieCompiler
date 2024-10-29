
namespace LUIECompiler.Common.Errors
{
    /// <summary>
    /// Represents an error where a register is defined multiple times.
    /// </summary>
    public class UseOfGuardError : IdentifierError
    {
        /// <summary>
        /// Creates a new redefine error.
        /// </summary>
        /// <param name="context">Context where the register was redefined.</param>
        /// <param name="identifier">Identifier that is already defined.</param>
        public UseOfGuardError(ErrorContext context, string identifier) : base(context, identifier)
        {
            Type = ErrorType.Critical;
            Description = $"The guard {identifier} cannot be used in the scope it is guarding.";
        }
    }

}
