
namespace LUIECompiler.Common.Errors
{
    /// <summary>
    /// Represents an error where a gate is given the wrong number of arguments.
    /// </summary>
    public class InvalidArguments : CompilationError
    {
        /// <summary>
        /// Gate with the wrong number of arguments.
        /// </summary>
        public IGate Gate { get; init; }

        /// <summary>
        /// Given number of arguments.
        /// </summary>
        public int NumberOfArguments { get; init; }
        
        /// <summary>
        /// Creates a new invalid arguments error.
        /// </summary>
        /// <param name="context">Context where the gate was called.</param>
        /// <param name="gate">Gate with the wrong number of arguments.</param>
        /// <param name="numberOfArguments">Given number of arguments.</param>
        public InvalidArguments(ErrorContext context, IGate gate, int numberOfArguments)
        {
            Type = ErrorType.Critical;
            ErrorContext = context;
            Gate = gate;
            NumberOfArguments = numberOfArguments;
            Description = $"The gate '{Gate}' takes {Gate.NumberOfArguments} arguments, but received {NumberOfArguments}.";
        }
    }

}