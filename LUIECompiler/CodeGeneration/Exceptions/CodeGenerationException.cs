
using LUIECompiler.Common.Errors;

namespace LUIECompiler.CodeGeneration.Exceptions
{
    /// <summary>
    /// Code generation exception that is cause be invalid source code. 
    /// </summary>
    public class CodeGenerationException : Exception
    {
        /// <summary>
        /// Error that caused the exception.
        /// </summary>
        public required CompilationError Error { get; init; }
    }

}