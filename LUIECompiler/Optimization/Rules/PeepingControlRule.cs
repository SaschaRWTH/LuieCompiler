using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Optimization.Graphs;
using LUIECompiler.Optimization.Graphs.Interfaces;
using LUIECompiler.Optimization.Graphs.Nodes;

namespace LUIECompiler.Optimization.Rules
{
    /// <summary>
    /// Null gate optimization rule.
    /// </summary>
    public class PeepingControlRule : OptimizationRule
    {
        public override int MaxRuleDepth => 1;

        public static readonly PeepingControlRule Rule = new();

        /// <summary>
        /// Creates a new instance of the PeepingControlRule.
        /// </summary>
        public PeepingControlRule()
        {
        }

        /// <summary>
        /// Saves the evaluation result of the rule.
        /// </summary>
        private bool? EvaluationResult = null;


        /// <summary>
        /// Applies the rule to the given <paramref name="path"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="InternalException"></exception>
        public override void Apply(WirePath path)
        {
            if (path.Length != 1)
            {
                throw new InternalException
                {
                    Reason = "The path must have exactly one node.",
                };
            }
            INode node = path.Nodes[0];

            if (node is not GateNode gateNode)
            {
                throw new InternalException
                {
                    Reason = "Node is not a gate node.",
                };
            }

            if (EvaluationResult == null)
            {
                throw new InternalException
                {
                    Reason = "Evaluation result is not set.",
                };
            }

            GraphGuard guard = gateNode.GetGuardQubits().Single(guard => guard.Qubit == path.Qubit);

            bool willTrigger = EvaluationResult.Value != guard.Negated;

            if (!willTrigger)
            {
                gateNode.Remove();
            }
        }

        /// <summary>
        /// Indicates whether the rule is applicable to the given <paramref name="path"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public override bool IsApplicable(WirePath path)
        {
            if (path.Length != 1)
            {
                return false;
            }
            INode node = path.Nodes[0];

            if (node is not GateNode gateNode)
            {
                return false;
            }

            if (!QubitIsControl(path.Qubit, gateNode))
            {
                return false;
            }

            List<GateNode> nodes = gateNode.NodesUpTo(path);

            if (!TryEvaluate(nodes, out bool qubitState))
            {
                return false;
            }

            EvaluationResult = qubitState;
            return true;
        }

        /// <summary>
        /// Indicates whether the qubit is a control qubit of the gate.
        /// </summary>
        /// <param name="qubit"></param>
        /// <param name="gateNode"></param>
        /// <returns></returns>
        private bool QubitIsControl(GraphQubit qubit, GateNode gateNode)
        {
            HashSet<GraphGuard> guards = gateNode.GetGuardQubits();
            return guards.Any(guard => guard.Qubit == qubit);
        }

        /// <summary>
        /// Tries to evaluate the given <paramref name="nodes"/> and returns the result in <paramref name="qubitState"/>.
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="qubitState"></param>
        /// <returns>Returns false if the evaluation is not possible.</returns>
        private bool TryEvaluate(List<GateNode> nodes, out bool qubitState)
        {
            qubitState = false;

            foreach (GateNode node in nodes)
            {
                if (node.Qubits.Count != 1)
                {
                    return false;
                }

                if (node.Gate == Common.GateType.X)
                {
                    qubitState = !qubitState;
                }
                else if (node.Gate == Common.GateType.Z)
                {
                    // Do nothing
                }
                else if (node.Gate == Common.GateType.Y)
                {
                    qubitState = !qubitState;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            return $"PeepingControlRule";
        }
    }
}