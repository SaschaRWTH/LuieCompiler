using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common;
using LUIECompiler.Optimization.Graphs;
using LUIECompiler.Optimization.Graphs.Nodes;

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
        /// Nullgate rule for the consecutive application of two Y gates.
        /// </summary>
        public static readonly NullGateRule YYGateRule = new NullGateRule([GateType.Y, GateType.Y]);

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
            YYGateRule,
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
        /// Applies the Null Gate optimization rule to the given <paramref name="path"/> 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public override void Apply(WirePath path)
        {
            foreach (var node in path.Nodes)
            {
                if (node is not GateNode gateNode)
                {
                    throw new InternalException()
                    {
                        Reason = "Node is not a gate node."
                    };
                }
                gateNode.Remove();
            }
        }

        /// <summary>
        /// Checks whether the Null Gate optimization rule is applicable to the given <paramref name="code"/>.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// <exception cref="InternalException"></exception>
        public override bool IsApplicable(WirePath path)
        {
            if (path.Length != NullGateCombination.Length)
            {
                return false;
            }

            return FollowGateCombination(path) && OperateOnSameQubit(path);
        }

        /// <summary>
        /// Checks whether the given <paramref name="path"/> follows the gate combination of the NullGateRule.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool FollowGateCombination(WirePath path)
        {
            for (int i = 0; i < NullGateCombination.Length; i++)
            {
                if (path.Nodes[i] is not GateNode gateNode)
                {
                    return false;
                }

                if (gateNode.Gate != NullGateCombination[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks whether all gates in the given <paramref name="path"/> operate on the same qubits.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool OperateOnSameQubit(WirePath path)
        {
            // Casting to GateNode
            if (path.Nodes[0] is not GateNode baseGate)
            {
                return false;
            }
            for (int i = 1; i < NullGateCombination.Length; i++)
            {
                // Casting to GateNode
                if (path.Nodes[i] is not GateNode gateNode)
                {
                    return false;
                }
                
                // Check if the gates have the same amount of parameters.
                if (baseGate.GateCode.Parameters.Count != gateNode.GateCode.Parameters.Count)
                {
                    return false;
                }

                // Check mutually inclusive semantical equality of the guards.
                // For guards, the order does not matter.
                // Check if for any guard in the base gate, there exists a semantically equal guard in the current gate.
                foreach (var qubit in baseGate.GateCode.Guards)
                {
                    if (!gateNode.GateCode.Guards.Any(guard => guard.SemanticallyEqual(qubit)))
                    {
                        return false;
                    }
                }
                // Check if for any guard in the current gate, there exists a semantically equal guard in the base gate.
                foreach (var qubit in gateNode.GateCode.Guards)
                {
                    if (!baseGate.GateCode.Guards.Any(guard => guard.SemanticallyEqual(qubit)))
                    {
                        return false;
                    }
                }

                // Check if all parameters are semantically equal.
                for (int j = 0; j < baseGate.GateCode.Parameters.Count; j++)
                {
                    if (!baseGate.GateCode.Parameters[j].SemanticallyEqual(gateNode.GateCode.Parameters[j]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public override string ToString()
        {
            return $"NullGateRule: {string.Join(", ", NullGateCombination)}";
        }
    }
}