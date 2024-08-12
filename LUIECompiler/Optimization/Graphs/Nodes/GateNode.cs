
using LUIECompiler.Common;

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

    }
}