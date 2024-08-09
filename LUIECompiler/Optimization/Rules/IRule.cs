using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.Optimization.Rules
{
    
    public interface IRule
    {
        public abstract bool IsApplicable(Code code);

        public abstract Code Apply(Code code);
    }
}