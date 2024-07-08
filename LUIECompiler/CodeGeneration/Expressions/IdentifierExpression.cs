namespace LUIECompiler.CodeGeneration.Expressions
{
    public class IdentifierExpression<T> : Expression<T>
    {
        public required string Identifier { get; init; }

        public override T Evaluate() 
        { 
            throw new NotImplementedException();
        }
    }
}