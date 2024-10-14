
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

            foreach (GraphQubit qubit in qubits)
            {
                qubit.AddGateNode(this);
            }
        }

        public override string ToString()
        {
            return $"GateNode = {{ Gate = {Gate}, Inputs = {InputVertices.Count}, Outputs = {OutputVertices.Count} }}";
        }

        /// <summary>
        /// Removes the node from the graph.
        /// </summary>
        public void Remove()
        {
            foreach (GraphQubit qubit in Qubits)
            {
                CircuitVertex inVertex = GetInVertex(qubit) as CircuitVertex ?? throw new InternalException()
                {
                    Reason = $"The input vertex is missing for qubit {qubit} or is not a Circuit Vertex."
                };
                IVertex outVertex = GetOutVertex(qubit) ?? throw new InternalException()
                {
                    Reason = $"The output vertex is missing for qubit {qubit}."
                };

                inVertex.ExtendTo(outVertex.End);
            }

            Graph.RemoveNode(this);
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
            if (GateCode.Gate.GateType == GateType.CX)
            {
                GraphQubit qubit = circuitGraph.FromCodeToQubit(GateCode.Parameters[0]);
                Compiler.LogInfo($"Adding guard qubit {qubit} to gate {GateCode.Gate}.");
                guards.Add(new GraphGuard(qubit, false, this));
            }

            foreach (GuardCode code in GateCode.Guards)
            {
                GraphQubit qubit = circuitGraph.FromCodeToQubit(code.Qubit);

                // Should no longer be the case, check to be safe.
                if (guards.Any(g => g.Qubit == qubit))
                {
                    Compiler.LogWarning("Gate was controlled by the same qubit multiple times, should not be possible. Optimization can continue.");
                    continue;
                }

                Compiler.LogInfo($"Adding guard qubit {qubit} to gate {GateCode.Gate}.");
                guards.Add(new GraphGuard(qubit, code.Negated, this));
            }


            return guards;
        }

        /// <summary>
        /// Gets the parameter qubits of the gate.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InternalException"></exception>
        public List<GraphQubit> GetParameters()
        {
            if (Graph is not CircuitGraph circuitGraph)
            {
                throw new InternalException()
                {
                    Reason = "The graph is not a circuit graph."
                };
            }

            return [.. GateCode.Parameters.Select(circuitGraph.FromCodeToQubit)];
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
            IVertex vertex = GetInVertex(qubit) ?? throw new InternalException()
            {
                Reason = $"The input vertex is missing for qubit {qubit}."
            };

            if (vertex.Start is not GateNode node)
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

            if (GateCode.Guards.Contains(guard!.Code))
            {
                GateCode.Guards.Remove(guard!.Code);
                return;
            }
            
            // Special case for CX gate
            if (GateCode.Gate.GateType != GateType.CX)
            {
                throw new InternalException()
                {
                    Reason = $"The qubit {qubit} is not a guard for the gate and can, therefore, not be removed as such."
                };
            }

            /// Create 

        }
    }
}