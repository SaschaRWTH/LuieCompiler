
using Antlr4.Runtime;

namespace LUIECompiler.Common.Errors
{

    public enum ErrorType
    {
        Warning,
        Critical,
    }

    public abstract class CompilationError
    {
        /// <summary>
        /// Type of the error.
        /// </summary>
        public ErrorType Type { get; init; }

        /// <summary>
        /// Line where the error occured.
        /// </summary>
        public int Line { get; init; }

        /// <summary>
        /// Description of the error.
        /// </summary>
        public string Description { get; set; } = "";

        public override string ToString()
        {
            return $"A {(Type == ErrorType.Warning ? "Warning" : "critical Error")} occured in line {Line}: {Description}";
        }
    }

}
