
using LUIECompiler.Optimization.Sequences;

namespace LUIECompiler.Optimization.Rules
{
    /// <summary>
    /// Abstract optimization rule.
    /// </summary>
    public abstract class OptimizationRule : IRule
    {
        /// <summary>
        /// Checks if the optimization rule is applicable to the given <paramref name="code"/>.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public abstract bool IsApplicable(CodeSequence code);

        /// <summary>
        /// Applies the optimization rule to the given <paramref name="code"/>.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public abstract CodeSequence Apply(CodeSequence code);
    }

}