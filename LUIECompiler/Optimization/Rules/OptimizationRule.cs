using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.Optimization.Rules
{
    public abstract class OptimizationRule : IRule
    {
        public abstract bool IsApplicable(Code code);

        public abstract Code Apply(Code code);
    }

}