
using LUIECompiler.Common;
using LUIECompiler.Optimization.Graphs.Interfaces;

namespace LUIECompiler.Optimization.Graphs.Nodes
{
    public class GateNode : Node
    {
        public override List<IVertex> InputVertices { get; } = [];

        public override List<IVertex> OutputVertices { get; } = [];
        
        public GateType Gate { get; }

        public GateNode(IGraph graph, GateType gate, List<GraphQubit> qubits): base(graph)
        {
            Gate = gate;
            
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