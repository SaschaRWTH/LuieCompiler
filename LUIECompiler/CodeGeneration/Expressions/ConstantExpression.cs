using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Expressions
{
    public class ConstantExpression<T> : Expression<T>
    {
        public required T Value { get; init; }

        public override T Evaluate(List<Constant<T>> constants) 
        { 
            return Value;
        }
    }
}