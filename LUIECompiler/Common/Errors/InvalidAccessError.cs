namespace LUIECompiler.Common.Errors
{
    /// <summary>
    /// Error that occurs when a register is accessed at an invalid index.
    /// The error can be thrown because any access is known at compile time. 
    /// However, the error is thrown when the index expression is evaluated 
    /// and can, therefore, not be thrown in the semantic analysis phase. 
    /// </summary>
    public class InvalidAccessError : CompilationError
    {
        /// <summary>
        /// Identifier that was accessed.
        /// </summary>
        public string Identifier { get; init; }

        /// <summary>
        /// Inde that was used to access the register.
        /// </summary>
        public int InvalidIndex { get; init; } 

        /// <summary>
        /// Size of the register that was accessed.
        /// </summary>
        public int RegisterSize { get; init; }

        /// <summary>
        /// Creates a new invalid access error.
        /// </summary>
        /// <param name="context">Context of the register access.</param>
        /// <param name="identifier">Identifier of the register that was accessed.</param>
        /// <param name="invalidIndex">Index that was used to access the register.</param>
        /// <param name="registerSize">Size of the register that was accessed.</param>
        public InvalidAccessError(ErrorContext context, string identifier, int invalidIndex, int registerSize)
        {
            Type = ErrorType.Critical;
            ErrorContext = context;
            Identifier = identifier;
            InvalidIndex = invalidIndex;
            RegisterSize = registerSize;
            Description = $"The register {identifier} was accessed at index {invalidIndex} but has a size of {registerSize}.";
        }
    }
}