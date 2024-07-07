namespace LUIECompiler.CodeGeneration.Expressions
{
    public class ConstantExpression<T> : Expression<T>
    {
        public required T Value { get; init; }

        public override T Evaluate() 
        { 
            return Value;
        }
    }
}