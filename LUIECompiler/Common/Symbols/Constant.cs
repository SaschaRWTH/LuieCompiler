namespace LUIECompiler.Common.Symbols
{
    public class Constant<T> : Symbol
    {
        /// <summary>
        /// The value of the constant.
        /// </summary>
        public required T Value { get; init; }

        public Constant(string identifier, T value) : base(identifier)
        {
            Value = value;
        }
    }
}