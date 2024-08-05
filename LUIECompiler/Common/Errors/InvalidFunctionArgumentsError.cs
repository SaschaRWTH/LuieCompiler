
namespace LUIECompiler.Common.Errors
{
    /// <summary>
    /// Represents an error where a function is called with the wrong number of arguments.
    /// </summary>
    public class InvalidFunctionArguments : CompilationError
    {
        /// <summary>
        /// Gate with the wrong number of arguments.
        /// </summary>
        public string Function { get; init; }

        /// <summary>
        /// Required number of arguments.
        /// </summary>
        public int Required { get; init; }

        /// <summary>
        /// Given number of arguments.
        /// </summary>
        public int Given { get; init; }
        
        /// <summary>
        /// Creates a new invalid function arguments error.
        /// </summary>
        /// <param name="context">Context where the function was called.</param>
        /// <param name="function">Function with the wrong number of arguments.</param>
        /// <param name="required">Required number of arguments.</param>
        /// <param name="given">Given number of arguments.</param>
        public InvalidFunctionArguments(ErrorContext context, string function, int required, int given)
        {
            Type = ErrorType.Critical;
            ErrorContext = context;
            Function = function;
            Required = required;
            Given = given;  
            Description = $"The function '{Function}' takes {Required} arguments, but received {Given}.";
        }
    }

}