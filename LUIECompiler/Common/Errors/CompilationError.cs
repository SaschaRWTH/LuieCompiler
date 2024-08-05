
namespace LUIECompiler.Common.Errors
{
    /// <summary>
    /// Error types.
    /// </summary>
    public enum ErrorType
    {
        Warning,
        Critical,
    }

    /// <summary>
    /// Represents an abstract error that occured during compilation.
    /// </summary>
    public abstract class CompilationError
    {
        /// <summary>
        /// Type of the error.
        /// </summary>
        public ErrorType Type { get; init; }

        /// <summary>
        /// Line where the error occured.
        /// </summary>
        public ErrorContext ErrorContext { get; init; }

        /// <summary>
        /// Description of the error.
        /// </summary>
        public string Description { get; set; } = "";

        public override string ToString()
        {
            return $"A {(Type == ErrorType.Warning ? "Warning" : "critical Error")} occured at ({ErrorContext.Line}, {ErrorContext.Column}): {Description}";
        }
    }

}
