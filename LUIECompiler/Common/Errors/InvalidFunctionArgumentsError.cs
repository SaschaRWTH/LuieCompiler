using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.CodeGeneration.Expressions;

namespace LUIECompiler.Common.Errors
{
    public class InvalidFunctionArguments : CompilationError
    {
        /// <summary>
        /// Gate with the wrong number of arguments.
        /// </summary>
        public FunctionType Function { get; init; }

        /// <summary>
        /// Required number of arguments.
        /// </summary>
        public int Required { get; init; }

        /// <summary>
        /// Given number of arguments.
        /// </summary>
        public int Given { get; init; }
        
        public InvalidFunctionArguments(ErrorContext context, FunctionType function, int required, int given)
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