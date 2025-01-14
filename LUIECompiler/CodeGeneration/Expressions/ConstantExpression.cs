using System.Numerics;
using LUIECompiler.Common;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Expressions
{
    /// <summary>
    /// Represents a constant expression.
    /// </summary>
    /// <typeparam name="T">Type of the constant.</typeparam>
    public class ConstantExpression<T> : Expression<T> where T : INumber<T>
    {
        /// <summary>
        /// Value of the constant.
        /// </summary>
        public required T Value { get; init; }

        public override T Evaluate(CodeGenerationContext context) 
        { 
            return Value;
        }

        public override List<string> PropagateSymbolInformation(SymbolTable table)
        {
            return new();
        }

        public override string ToString()
        {
            return Value.ToString() ?? "null";
        }
    }
}