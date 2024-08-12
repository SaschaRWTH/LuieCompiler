using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Optimization.Graphs.Nodes;

namespace LUIECompiler.Optimization.Graphs
{
    public class CircuitGraph : IGraph
    {
        public List<INode> Nodes { get; } = [];
        public List<IVertex> Vertices { get; } = [];

        public List<InputNode> InputNodes => Nodes.OfType<InputNode>().ToList();

        public List<OutputNode> OutputNodes => Nodes.OfType<OutputNode>().ToList();

        public List<GraphQubit> Qubits { get; } = [];

        public CircuitGraph(QASMProgram program)
        {
            var definitions = program.Code.OfType<DefinitionCode>();
            var gates = program.Code.OfType<GateApplicationCode>();

            CreateQubits(definitions);
            AddGates(gates);
        }

        public void CreateQubits(IEnumerable<DefinitionCode> code)
        {
            foreach (DefinitionCode definition in code)
            {
                GraphQubit qubit = new(this, definition.Identifier.Identifier);
                Qubits.Add(qubit);
            }
        }

        public void AddGates(IEnumerable<GateApplicationCode> code)
        {
            foreach (GateApplicationCode gate in code)
            {
                var guardIds = gate.Guards.Select(p => p.Qubit.Identifier.Identifier);
                var parameterIds = gate.Parameters.Select(p => p.Identifier.Identifier);
                List<GraphQubit> qubits = GetQubits(guardIds.Concat(parameterIds));
                new GateNode(this, gate.Gate.GateType, qubits);
            }
        }

        public List<GraphQubit> GetQubits(IEnumerable<string> identifiers)
        {
            var qubits = Qubits.Where(q => identifiers.Contains(q.Identifier)).ToList(); 

            // TODO: This will fail if a qubit is used as parameter or guard
            // multiple times 
            if(qubits.Count != identifiers.Count())
            {
                throw new ArgumentException("Not all qubits were found in the graph");
            }

            return qubits;
        }

        public void AddNode(INode node)
        {
            Nodes.Add(node);
        }

        public void AddVertex(IVertex vertex)
        {
            Vertices.Add(vertex);
        }

        public void ReplacePath(Path old, Path replacement)
        {
            if (old.Start != replacement.Start || old.End != replacement.End)
            {
                throw new ArgumentException("Old path start/end nodes do not match replacement start/end nodes");
            }

            foreach (INode node in old.InnerNodes)
            {
                Nodes.Remove(node);
            }

            foreach (INode node in replacement.InnerNodes)
            {
                Nodes.Add(node);
            }

            foreach (IVertex vertex in old.Vertices)
            {
                Vertices.Remove(vertex);
            }

            foreach (IVertex vertex in replacement.Vertices)
            {
                Vertices.Add(vertex);
            }
        }
    }
}