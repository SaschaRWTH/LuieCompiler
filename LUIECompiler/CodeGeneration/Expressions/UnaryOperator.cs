using System.Numerics;

namespace LUIECompiler.CodeGeneration.Expressions
{
    public enum UnaryOperatorType
    {
        AdditiveInverse,
    }

    public class UnaryOperator<T> where T : INumber<T>
    {
        /// <summary>
        /// Type of the operator.
        /// </summary>
        public required UnaryOperatorType Type { get; init; }

        /// <summary>
        /// Creates an operator from a string.
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static UnaryOperator<T> FromString(string op)
        {
            return op switch
            {
                "-" => new UnaryOperator<T> { Type = UnaryOperatorType.AdditiveInverse },
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// Applies the operator to two numbers.
        /// </summary>
        /// <param name="operand"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public T Apply(T operand)
        {
            return Type switch
            {
                UnaryOperatorType.AdditiveInverse => -operand,
                _ => throw new NotImplementedException(),
            };
        }

        public override string ToString()
        {
            return Type switch
            {
                UnaryOperatorType.AdditiveInverse => "-",
                _ => throw new NotImplementedException(),
            };
        }
    }
}