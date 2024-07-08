namespace LUIECompiler.Common.Symbols
{
    public class Constant<T> : Symbol
    {
        /// <summary>
        /// The value of the constant.
        /// </summary>
        public T Value { get; init; }

        public Constant(string identifier, T value) : base(identifier)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"Constant: [Identifier = {Identifier}, Value = {Value}]";
        }
    }
}