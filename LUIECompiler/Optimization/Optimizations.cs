using LUIECompiler.Optimization.Rules;

namespace LUIECompiler.Optimization
{
    [Flags]
    public enum OptimizationType
    {
        NullGate = 0b0000_0001,
        PeepingControl = 0b0000_0010,

        All = NullGate | PeepingControl,
    }

    public static class OptimizationTypeExtension
    {
        public static List<OptimizationRule> GetRules(this OptimizationType type)
        {
            List<OptimizationRule> rules = [];

            if(type.HasFlag(OptimizationType.NullGate))
            {
                rules.AddRange(NullGateRule.NullGateRules);
            }

            if (type.HasFlag(OptimizationType.PeepingControl))
            {
                rules.Add(PeepingControlRule.Rule);
            }

            return rules;
        }
    }
}