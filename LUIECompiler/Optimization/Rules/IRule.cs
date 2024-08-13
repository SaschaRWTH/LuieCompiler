using LUIECompiler.Optimization.Graphs;

namespace LUIECompiler.Optimization.Rules
{
    /// <summary>
    /// Interface for rules.
    /// </summary>
    public interface IRule
    {
        /// <summary>
        /// Indicates the maximum length of code sequences to check.
        /// </summary>
        public static int MaxRuleDepth { get; }

        /// <summary>
        /// Checks if the rule is applicable to the given <paramref name="code"/>.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public abstract bool IsApplicable(WirePath path);

        /// <summary>
        /// Applies the rule to the given <paramref name="code"/>.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public abstract void Apply(WirePath path);
    }
}