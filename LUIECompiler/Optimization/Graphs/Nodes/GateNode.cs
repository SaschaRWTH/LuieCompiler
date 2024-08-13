
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common;
using LUIECompiler.Optimization.Graphs.Interfaces;

namespace LUIECompiler.Optimization.Graphs.Nodes
{
    /// <summary>
    /// A node that reprensent a gate in a quantum circuit graph.
    /// </summary>
    public class GateNode : Node
    {
        /// <summary>
        /// The type of the gate.
        /// </summary>
        public GateType Gate => GateCode.Gate.GateType;

        public GateApplicationCode GateCode { get; }

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
                Console.WriteLine($"Removing gate {Gate} from qubit {qubit}");
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
    }
}