using System.Numerics;

namespace LUIECompiler.CodeGeneration.Expressions
{
    public enum BinaryOperatorType
    {
        Add,
        Subtract,
        Multiply,
        Divide,
    }

    public class BinaryOperator<T> where T : INumber<T>
    {
        /// <summary>
        /// Type of the operator.
        /// </summary>
        public required BinaryOperatorType Type { get; init; }

        /// <summary>
        /// Creates an operator from a string.
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static BinaryOperator<T> FromString(string op)
        {
            return op switch
            {
                "+" => new BinaryOperator<T> { Type = BinaryOperatorType.Add },
                "-" => new BinaryOperator<T> { Type = BinaryOperatorType.Subtract },
                "*" => new BinaryOperator<T> { Type = BinaryOperatorType.Multiply },
                "/" => new BinaryOperator<T> { Type = BinaryOperatorType.Divide },
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// Applies the operator to two numbers.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public T Apply(T left, T right)
        {
            return Type switch
            {
                BinaryOperatorType.Add => left + right,
                BinaryOperatorType.Subtract => left - right,
                BinaryOperatorType.Multiply => left * right,
                BinaryOperatorType.Divide => left / right,
                _ => throw new NotImplementedException(),
            };
        }
    }
}