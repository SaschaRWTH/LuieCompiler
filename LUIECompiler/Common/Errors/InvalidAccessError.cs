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

        public int InvalidIndex { get; init; } 

        public int RegisterSize { get; init; }

        public InvalidAccessError(int line, string identifier, int invalidIndex, int registerSize)
        {
            Type = ErrorType.Critical;
            Line = line;
            Identifier = identifier;
            InvalidIndex = invalidIndex;
            RegisterSize = registerSize;
            Description = $"The register {identifier} was accessed at index {invalidIndex} but has a size of {registerSize}.";
        }
    }
}