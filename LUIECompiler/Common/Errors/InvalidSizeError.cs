namespace LUIECompiler.Common.Errors
{
    /// <summary>
    /// Represents an error where a register has an invalid size.
    /// </summary>
    public class InvalidSizeError : IdentifierError
    {
        /// <summary>
        /// Size of the register.
        /// </summary>
        public int Size { get; init; }
        
        /// <summary>
        /// Creates a new invalid size error.
        /// </summary>
        /// <param name="context">Context where the register was declared.</param>
        /// <param name="identifier">Name of the register.</param>
        /// <param name="registerSize">(Invalid) Size of the register.</param>
        public InvalidSizeError(ErrorContext context, string identifier, int registerSize) : base(context, identifier)
        {
            Type = ErrorType.Critical;
            Size = registerSize;
            Description = $"The register '{Identifier}' has an invalid size of {Size}. The size must be a positive integer.";
        }
    }
}