
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common;
using LUIECompiler.Optimization.Graphs.Interfaces;

namespace LUIECompiler.Optimization.Graphs.Nodes
{
    /// <summary>
    /// A node that represent a gate in a quantum circuit graph.
    /// </summary>
    public class GateNode : Node
    {
        /// <summary>
        /// The type of the gate.
        /// </summary>
        public GateType Gate => GateCode.Gate.GateType;

        public GateApplicationCode GateCode { get; private set; }

        /// <summary>
        /// Gets the qubits of the gate.
        /// </summary>
        public List<GraphQubit> Qubits { get; }

        /// <summary>
        /// Creates a new gate node.
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="gate"></param>
        /// <param name="qubits"></param>
        public GateNode(CircuitGraph graph, GateApplicationCode gate, List<GraphQubit> qubits) : base(graph)
        {
            Qubits = qubits;
            GateCode = gate;
        }

        /// <summary>
        /// Adds the node to the graph by adding the gate node to all qubits that are used as input.
        /// </summary>
        public void AddToGraph()
        {
            foreach (GraphQubit qubit in Qubits)
            {
                qubit.AddGateNode(this);
            }
        }

        public override string ToString()
        {
            return $"GateNode = {{ Gate = {Gate}, Inputs = {InputEdges.Count}, Outputs = {OutputEdges.Count} }}";
        }

        /// <summary>
        /// Removes the node from the graph.
        /// </summary>
        public void Remove()
        {
            // Create copy of Qubits to avoid concurrent modification.
            IEnumerable<GraphQubit> qubits = [ .. Qubits];
            foreach (GraphQubit qubit in qubits)
            {
                RemoveSingleQubit(qubit);
            }

            Graph.RemoveNode(this);
        }

        public void RemoveSingleQubit(GraphQubit qubit)
        {
            CircuitEdge inEdge = GetInEdge(qubit) as CircuitEdge ?? throw new InternalException()
            {
                Reason = $"The input edge is missing for qubit {qubit} or is not a Circuit Edge."
            };
            IEdge outEdge = GetOutEdge(qubit) ?? throw new InternalException()
            {
                Reason = $"The output edge is missing for qubit {qubit}."
            };
            inEdge.ExtendTo(outEdge.End);

            Qubits.Remove(qubit);
        }

        /// <summary>
        /// Gets a set of qubits that are used as guards for the gate.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InternalException"></exception>
        public HashSet<GraphGuard> GetGuardQubits()
        {
            if (Graph is not CircuitGraph circuitGraph)
            {
                throw new InternalException()
                {
                    Reason = "The graph is not a circuit graph."
                };
            }

            HashSet<GraphGuard> guards = [];
            // Special case for CX gate
            // This special case should no longer be needed, all CX and CCX gates translated to controlled X gates.
            // if (GateCode.Gate.GateType == GateType.CX)
            // {
            //     GraphQubit qubit = circuitGraph.FromCodeToQubit(GateCode.Argument[0]);
            //     Compiler.LogInfo($"Adding guard qubit {qubit} to gate {GateCode.Gate}.");
            //     guards.Add(new GraphGuard(qubit, false, this));
            // }

            foreach (GuardCode code in GateCode.Guards)
            {
                GraphQubit qubit = circuitGraph.FromCodeToQubit(code.Qubit);

                // Should no longer be the case, check to be safe.
                if (guards.Any(g => g.Qubit == qubit))
                {
                    Compiler.LogWarning("Gate was controlled by the same qubit multiple times, should not be possible. Optimization can continue.");
                    continue;
                }

                guards.Add(new GraphGuard(qubit, code.Negated, this));
            }


            return guards;
        }

        /// <summary>
        /// Gets the argument qubits of the gate.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InternalException"></exception>
        public List<GraphQubit> GetArguments()
        {
            if (Graph is not CircuitGraph circuitGraph)
            {
                throw new InternalException()
                {
                    Reason = "The graph is not a circuit graph."
                };
            }

            return [.. GateCode.Arguments.Select(circuitGraph.FromCodeToQubit)];
        }

        /// <summary>
        /// Gets all previous gate nodes in order of application on the same wire.
        /// </summary>
        /// <param name="wire"></param>
        /// <returns></returns>
        /// <exception cref="InternalException"></exception>
        public List<GateNode> NodesUpTo(WirePath wire)
        {
            GraphQubit qubit = wire.Qubit;
            IEdge edge = GetInEdge(qubit) ?? throw new InternalException()
            {
                Reason = $"The input edge is missing for qubit {qubit}."
            };

            if (edge.Start is not GateNode node)
            {
                return [];
            }

            var result = node.NodesUpTo(wire);
            result.Add(node);
            return result;
        }

        public void ReplaceGate(GateApplicationCode gate)
        {
            GateCode = gate;
        }

        public bool TryGetGuard(GraphQubit qubit, out GraphGuard? guard)
        {
            HashSet<GraphGuard> guards = GetGuardQubits();
            guard = guards.FirstOrDefault(g => g.Qubit == qubit);

            return guard != null;
        }

        public void RemoveAsGuard(GraphQubit qubit)
        {
            if (!TryGetGuard(qubit, out GraphGuard? guard))
            {
                throw new InternalException()
                {
                    Reason = $"The qubit {qubit} is not a guard for the gate and can, therefore, not be removed as such."
                };
            }

            GateCode.Guards.Remove(guard!.Code);
            RemoveSingleQubit(qubit);

            // Special case for CX gate
            // This special case should no longer be needed, all CX and CCX gates translated to controlled X gates.
            // if (GateCode.Gate.GateType != GateType.CX)
            // {
            //     throw new InternalException()
            //     {
            //         Reason = $"The qubit {qubit} is not a guard for the gate and can, therefore, not be removed as such."
            //     };
            // }

            /// Create 

        }
    }
}