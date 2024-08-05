namespace LUIECompiler.Common.Errors
{
    /// <summary>
    /// Represents an error where a register has an invalid size.
    /// </summary>
    public class InvalidSizeError : CompilationError
    {
        /// <summary>
        /// Name of the register.
        /// </summary>
        public string RegisterName { get; init; }

        /// <summary>
        /// Size of the register.
        /// </summary>
        public int RegisterSize { get; init; }
        
        /// <summary>
        /// Creates a new invalid size error.
        /// </summary>
        /// <param name="context">Context where the register was declared.</param>
        /// <param name="registerName">Name of the register.</param>
        /// <param name="registerSize">(Invalid) Size of the register.</param>
        public InvalidSizeError(ErrorContext context, string registerName, int registerSize)
        {
            Type = ErrorType.Critical;
            ErrorContext = context;
            RegisterName = registerName;
            RegisterSize = registerSize;
            Description = $"The register '{RegisterName}' has an invalid size of {RegisterSize}. The size must be a positive integer.";
        }
    }
}