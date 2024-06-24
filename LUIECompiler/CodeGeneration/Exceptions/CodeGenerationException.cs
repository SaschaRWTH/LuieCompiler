
using LUIECompiler.Common.Errors;

namespace LUIECompiler
{ 
    public class CodeGenerationException : Exception
    {
        public required CompilationError Error { get; init; }
    }

}