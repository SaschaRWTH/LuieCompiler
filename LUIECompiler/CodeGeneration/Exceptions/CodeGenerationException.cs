
using LUIECompiler.Common.Errors;

namespace LUIECompiler.CodeGeneration.Exceptions
{
    /// <summary>
    /// Code generation exception that is cause be invalid source code. 
    /// </summary>
    public class CodeGenerationException : Exception
    {
        public required CompilationError Error { get; init; }
    }

}