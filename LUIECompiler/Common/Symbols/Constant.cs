using LUIECompiler.Common.Errors;

namespace LUIECompiler.Common.Symbols
{
    /// <summary>
    /// Represents a constant.
    /// </summary>
    /// <typeparam name="T">Type of the constant.</typeparam>
    public class Constant<T> : Symbol
    {
        /// <summary>
        /// The value of the constant.
        /// </summary>
        public T Value { get; init; }

        /// <summary>
        /// Creates a new constant.
        /// </summary>
        /// <param name="identifier">Identifier of the constant.</param>
        /// <param name="value">Value of the constant.</param>
        /// <param name="errorContext">Context of the constant definition.</param>
        public Constant(string identifier, T value, ErrorContext errorContext) : base(identifier, errorContext)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"Constant: {{ Identifier = {Identifier}, Value = {Value} }}";
        }
    }
}