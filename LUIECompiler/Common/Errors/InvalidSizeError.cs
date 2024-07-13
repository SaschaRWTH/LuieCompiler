namespace LUIECompiler.Common.Errors
{
    public class InvalidSizeError : CompilationError
    {
        public string RegisterName { get; init; }
        public int RegisterSize { get; init; }
        
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