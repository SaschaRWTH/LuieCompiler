
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common;

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
        /// Creates a new gate node.
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="gate"></param>
        /// <param name="qubits"></param>
        public GateNode(CircuitGraph graph, GateApplicationCode gate, List<GraphQubit> qubits): base(graph)
        {
            GateCode = gate;
            
            foreach(GraphQubit qubit in qubits)
            {
                qubit.AddGateNode(this);
            }
        }

        public override string ToString()
        {
            return $"GateNode = {{ Gate = {Gate}, Inputs = {InputVertices.Count}, Outputs = {OutputVertices.Count} }}";
        }

    }
}