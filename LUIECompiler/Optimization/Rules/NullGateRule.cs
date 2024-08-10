using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common;

namespace LUIECompiler.Optimization.Rules
{
    /// <summary>
    /// Null gate optimization rule.
    /// </summary>
    public class NullGateRule : OptimizationRule
    {
        /// <summary>
        /// Indicates the maximum length of code sequences to check.
        /// </summary>
        public static int MaxRuleLength 
        {
            get => NullGateRules.Max(rule => rule.NullGateCombination.Length);
        }

        /// <summary>
        /// Nullgate rule for the consecutive application of two H gates.
        /// </summary>
        public static readonly NullGateRule HHGateRule = new NullGateRule([GateType.H, GateType.H]);

        /// <summary>
        /// Nullgate rule for the consecutive application of two X gates.
        /// </summary>
        public static readonly NullGateRule XXGateRule = new NullGateRule([GateType.X, GateType.X]);

        /// <summary>
        /// Nullgate rule for the consecutive application of two Z gates.
        /// </summary>
        public static readonly NullGateRule ZZGateRule = new NullGateRule([GateType.Z, GateType.Z]);

        /// <summary>
        /// List of all predefined NullGateRules.
        /// </summary>
        public static readonly List<NullGateRule> NullGateRules =
        [
            HHGateRule,
            XXGateRule,
            ZZGateRule
        ];

        /// <summary>
        /// The gate order that represents the null gate combination.
        /// </summary>
        public GateType[] NullGateCombination { get; }

        /// <summary>
        /// Creates a new instance of the NullGateRule with the given <paramref name="nullGateCombination"/>.
        /// </summary>
        /// <param name="nullGateCombination"></param>
        public NullGateRule(GateType[] nullGateCombination)
        {
            NullGateCombination = nullGateCombination;
        }

        /// <summary>
        /// Applies the Null Gate optimization rule to the given <paramref name="code"/> 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public override CodeSequence Apply(CodeSequence code)
        {
            if (!IsApplicable(code))
            {
                return code;
            }
            return new();
        }

        /// <summary>
        /// Checks whether the Null Gate optimization rule is applicable to the given <paramref name="code"/>.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// <exception cref="InternalException"></exception>
        public override bool IsApplicable(CodeSequence code)
        {
            if (code.IsEmpty || !code.OnlyGatesApplications)
            {
                return false;
            }

            if (code.Count != NullGateCombination.Length)
            {
                return false;
            }

            for (int i = 0; i < NullGateCombination.Length; i++)
            {
                if(code.Code[i] is not GateApplicationCode gateApplicationCode)
                {
                    throw new InternalException()
                    {
                        Reason = "'code.OnlyGatesApplications' was already check, it should not be possible that a code is not a GateApplicationCode."
                    };
                }

                if (gateApplicationCode.Gate.GateType != NullGateCombination[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}