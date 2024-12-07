using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common;
using LUIECompiler.Optimization.Graphs;
using LUIECompiler.Optimization.Graphs.Nodes;

namespace LUIECompiler.Optimization.Rules
{
    public class ControlReversalRule : OptimizationRule
    {
        public override int MaxRuleDepth => 1;

        public static readonly ControlReversalRule Rule = new();

        /// <summary>
        /// Saves the H gates to be removed when applying the rule.
        /// </summary>
        protected GateNode[] HGates { get; private set; } = [];

        public override void Apply(WirePath path)
        {
            if (path.Length != 1)
            {
                throw new InternalException()
                {
                    Reason = "The length of the path was not 1."
                };
            }

            if (path.Start is not GateNode node)
            {
                throw new InternalException()
                {
                    Reason = "The rule can only be applied to gate nodes."
                };
            }

            if (node.Gate == GateType.CX)
            {
                if (node.GateCode.Arguments.Count != 2)
                {
                    throw new InternalException()
                    {
                        Reason = $"The gate {node.GateCode} has an invalid number of arguments.",
                    };
                }

                QubitCode guard = node.GateCode.Arguments[0];
                QubitCode target = node.GateCode.Arguments[1];

                node.GateCode.Arguments[0] = target;
                node.GateCode.Arguments[1] = guard;
            }

            if (node.Gate == GateType.X)
            {
                if (node.GateCode.Arguments.Count != 1)
                {
                    throw new InternalException()
                    {
                        Reason = $"The gate {node.GateCode} has an invalid number of arguments.",
                    };
                }
                if (node.GateCode.Guards.Count != 1)
                {
                    throw new InternalException()
                    {
                        Reason = $"The gate {node.GateCode} has an invalid number of guards for the rule application.",
                    };
                }

                GuardCode guard = node.GateCode.Guards[0];
                QubitCode target = node.GateCode.Arguments[0];

                node.GateCode.Arguments[0] = guard.Qubit;
                node.GateCode.Guards[0] = new()
                {
                    Qubit = target,
                    Negated = false,
                };
            }

            foreach (GateNode hNode in HGates)
            {
                hNode.Remove();
            }
        }

        public override bool IsApplicable(WirePath path)
        {
            if (path.Length != 1)
            {
                return false;
            }

            if (path.Start is not GateNode node)
            {
                return false;
            }

            if (node.Gate == GateType.CX)
            {
                Compiler.LogError("Gate type cx should have been replaced in the code generation.");
                return false;
            }

            if (node.Gate == GateType.X)
            {
                return IsApplicableGuardedX(node);
            }


            return false;
        }

        /// <summary>
        ///  Checks if the rule is applicable to a guarded x gate.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool IsApplicableGuardedX(GateNode node)
        {
            HashSet<GraphGuard> guards = node.GetGuardQubits();
            if (guards.Count != 1)
            {
                return false;
            }

            GraphGuard guard = guards.First();
            // TODO: Check whether the rule can be applied in this case as well.
            if (guard.Negated)
            {
                return false;
            }

            List<GraphQubit> arguments = node.GetArguments();
            if (arguments.Count != 1)
            {
                return false;
            }

            return IsApplicableQubits(node, guard.Qubit, arguments[0]);
        }

        /// <summary>
        /// Checks if the rule is applicable to the <paramref name="node"/> with the given
        /// <paramref name="guard"/> and <paramref name="target"/>. 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="guard"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private bool IsApplicableQubits(GateNode node, GraphQubit guard, GraphQubit target)
        {
            if (node.GetPredecessor(guard) is not GateNode guardPredecessor || !IsSingleQubitHGate(guardPredecessor))
            {
                return false;
            }

            if (node.GetSuccessor(guard) is not GateNode guardSuccessor || !IsSingleQubitHGate(guardSuccessor))
            {
                return false;
            }

            if (node.GetPredecessor(target) is not GateNode targetPredecessor || !IsSingleQubitHGate(targetPredecessor))
            {
                return false;
            }

            if (node.GetSuccessor(target) is not GateNode targetSuccessor || !IsSingleQubitHGate(targetSuccessor))
            {
                return false;
            }

            // Save gates to prevent case distinction when applying rule
            // BEWARE: If optimization is parallelized, race conditions may apply!
            HGates =
            [
                guardPredecessor,
                guardSuccessor,
                targetPredecessor,
                targetSuccessor,
            ];

            return true;
        }

        /// <summary>
        /// Indicates whether a given <paramref name="node"/> is a single qubit H gate.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool IsSingleQubitHGate(GateNode node)
        {
            return node.Gate == GateType.H && node.GateCode.Guards.Count == 0;
        }
    }
}