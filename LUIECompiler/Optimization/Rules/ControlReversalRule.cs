using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common;
using LUIECompiler.Optimization.Graphs;
using LUIECompiler.Optimization.Graphs.Nodes;

namespace LUIECompiler.Optimization.Rules
{
    public class ControlReversalRule : OptimizationRule
    {
        public override int MaxRuleDepth => 1;

        public static readonly ControlReversalRule Rule = new();

        protected GateNode[] HGates { get; private set; } = [];

        public override void Apply(WirePath path)
        {
            throw new NotImplementedException();
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
                return IsApplicableCX(node);
            }

            if (node.Gate == GateType.X)
            {
                return IsApplicableGuardedX(node);
            }


            return false;
        }

        private bool IsApplicableCX(GateNode node)
        {
            if (node.GateCode.Guards.Count != 0)
            {
                return false;
            }

            List<GraphQubit> parameters = node.GetParameters();
            if (parameters.Count != 2)
            {
                return false;
            }

            GraphQubit guard = parameters[0];
            GraphQubit target = parameters[1];

            return IsApplicableQubits(node, guard, target);
        }

        private bool IsApplicableGuardedX(GateNode node)
        {
            HashSet<GraphGuard> guards = node.GetGuardQubits();
            if (guards.Count != 1)
            {
                return false;
            }

            GraphGuard guard = guards.First();
            // TODO: Check wheter the rule can be applied in this case as well.
            if (guard.Negated)
            {
                return false;
            }

            List<GraphQubit> parameters = node.GetParameters();
            if (parameters.Count != 1)
            {
                return false;
            }

            return IsApplicableQubits(node, guard.Qubit, parameters[0]);
        }

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

            if (node.GetSuccessor(target) is not GateNode targetPredecessor || !IsSingleQubitHGate(targetPredecessor))
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

        private bool IsSingleQubitHGate(GateNode node)
        {
            return node.Gate == GateType.H && node.GateCode.Guards.Count == 0;
        }
    }
}