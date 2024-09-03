using LUIECompiler.Optimization.Rules;

namespace LUIECompiler.Optimization
{
    /// <summary>
    /// Represents the type of optimization.
    /// </summary>
    [Flags]
    public enum OptimizationType
    {
        None = 0b0000_0000,

        NullGate = 0b0000_0001,
        PeepingControl = 0b0000_0010,
        HSandwichReduction = 0b0000_0100,
        ControlReversal = 0b0000_1000,

        All = NullGate | PeepingControl | HSandwichReduction | ControlReversal,
    }

    public static class OptimizationTypeExtension
    {
        /// <summary>
        /// Returns the rules for the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<OptimizationRule> GetRules(this OptimizationType type)
        {
            List<OptimizationRule> rules = [];

            if (type.HasFlag(OptimizationType.NullGate))
            {
                rules.AddRange(NullGateRule.NullGateRules);
            }

            if (type.HasFlag(OptimizationType.PeepingControl))
            {
                rules.Add(PeepingControlRule.Rule);
            }

            if (type.HasFlag(OptimizationType.HSandwichReduction))
            {
                rules.AddRange(HSandwichReductionRule.GateReductionRules);
            }

            if (type.HasFlag(OptimizationType.ControlReversal))
            {
                rules.Add(ControlReversalRule.Rule);
            }

            return rules;
        }

        /// <summary>
        /// Converts the <paramref name="type"/> to a command line input.
        /// </summary>
        public static string ToCommandLineInput(this OptimizationType type)
        {
            return type switch
            {
                OptimizationType.None => "none",
                OptimizationType.All => "all",
                OptimizationType.NullGate => "nullgate",
                OptimizationType.PeepingControl => "peepingcontrol",
                OptimizationType.HSandwichReduction => "hsandwich",
                OptimizationType.ControlReversal => "controlreversal",
                _ => throw new ArgumentException($"Unknown optimization: {type}"),
            };
        }

        /// <summary>
        /// Converts the command line input to an <see cref="OptimizationType"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static OptimizationType FromCommandLineInput(string type)
        {
            var values = Enum.GetValues(typeof(OptimizationType)).Cast<OptimizationType>();
            foreach (var value in values)
            {
                if (value.ToCommandLineInput() == type)
                {
                    return value;
                }
            }
            throw new ArgumentException($"Unknown optimization: {type}");
        }
    }
}