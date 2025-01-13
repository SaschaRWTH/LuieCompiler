
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Optimization.Graphs;
using LUIECompiler.Optimization.Graphs.Interfaces;
using LUIECompiler.Optimization.Graphs.Nodes;

namespace LUIECompiler.Optimization.Rules
{
    /// <summary>
    /// Abstract optimization rule.
    /// </summary>
    public abstract class OptimizationRule : IRule
    {
        /// <summary>
        /// Indicates the maximum length of code sequences to check.
        /// </summary>
        public abstract int MaxRuleDepth { get; }

        /// <summary>
        /// Checks if the optimization rule is applicable to the given <paramref name="path"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public abstract bool IsApplicable(WirePath path);

        /// <summary>
        /// Applies the optimization rule to the given <paramref name="path"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public abstract void Apply(WirePath path);

        
        /// <summary>
        /// Checks whether all gates in the given <paramref name="path"/> operate on the same qubits.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected bool OperateOnSameQubit(WirePath path)
        {
            // Casting to GateNode
            if (path.Nodes[0] is not GateNode baseGate)
            {
                return false;
            }
            for (int i = 1; i < path.Length; i++)
            {
                // Casting to GateNode
                if (path.Nodes[i] is not GateNode gateNode)
                {
                    return false;
                }

                // Check if the gates have the same amount of arguments.
                if (baseGate.GateCode.Arguments.Count != gateNode.GateCode.Arguments.Count)
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

                // Check if all arguments are semantically equal.
                for (int j = 0; j < baseGate.GateCode.Arguments.Count; j++)
                {
                    if (!baseGate.GateCode.Arguments[j].SemanticallyEqual(gateNode.GateCode.Arguments[j]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Check whether the gate nodes are consecutive for all qubits.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected bool ConsecutiveGatesForAllQubits(WirePath path)
        {
            // Casting to GateNode
            if (path.Nodes[0] is not GateNode baseGate)
            {
                return false;
            }

            GateNode current = baseGate;
            for (int i = 1; i < path.Length; i++)
            {
                GateNode nextNode = path.Nodes[i] as GateNode ?? throw new InternalException()
                {
                    Reason = "Node is not a gate node."
                };
                // Casting to GateNode
                foreach (GraphQubit qubit in baseGate.Qubits)
                {
                    INode nextNodeOfQubit = current.GetOutEdge(qubit).End;
                    if(nextNodeOfQubit != nextNode)
                    {
                        return false;
                    }
                }
                current = nextNode;
            }

            return true;
        }
    }

}