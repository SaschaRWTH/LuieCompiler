namespace LUIECompiler.CodeGeneration.Expressions
{
    public abstract class Expression<T> 
    {

        public abstract T Evaluate();
    }
}