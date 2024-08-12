using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.CodeGeneration.Definitions;
using LUIECompiler.Optimization.Graphs.Nodes;

namespace LUIECompiler.Optimization.Graphs
{
    /// <summary>
    /// Represents a quantum circuit graph.
    /// </summary>
    public class CircuitGraph : Graph
    {
        /// <summary>
        /// The input nodes of the graph.
        /// </summary>
        public List<InputNode> InputNodes => Nodes.OfType<InputNode>().ToList();

        /// <summary>
        /// The output nodes of the graph.
        /// </summary>
        public List<OutputNode> OutputNodes => Nodes.OfType<OutputNode>().ToList();

        /// <summary>
        /// The qubits of the graph.
        /// </summary>                
        public List<GraphQubit> Qubits { get; } = [];

        /// <summary>
        /// Creates a new quantum circuit graph.
        /// </summary>
        /// <param name="program">Quantum program from which to create the graph.</param>
        public CircuitGraph(QASMProgram program)
        {
            var definitions = program.Code.OfType<DefinitionCode>();
            var gates = program.Code.OfType<GateApplicationCode>();

            CreateQubits(definitions);
            AddGates(gates);
        }

        /// <summary>
        /// Creates the qubits of the graph.
        /// </summary>
        /// <param name="code"></param>
        public void CreateQubits(IEnumerable<DefinitionCode> code)
        {
            foreach (DefinitionCode definition in code)
            {
                GraphQubit qubit = new(this, definition.Identifier);
                Qubits.Add(qubit);
            }
        }

        /// <summary>
        /// Adds the gates to the graph.
        /// </summary>
        /// <param name="code"></param>
        public void AddGates(IEnumerable<GateApplicationCode> code)
        {
            foreach (GateApplicationCode gate in code)
            {
                var guardIds = gate.Guards.Select(p => p.Qubit.Identifier);
                var parameterIds = gate.Parameters.Select(p => p.Identifier);
                List<GraphQubit> qubits = GetQubits(guardIds.Concat(parameterIds));
                new GateNode(this, gate.Gate.GateType, qubits);
            }
        }

        /// <summary>
        /// Gets the qubits with the given <paramref name="identifiers"/>.
        /// </summary>
        /// <param name="identifiers"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public List<GraphQubit> GetQubits(IEnumerable<UniqueIdentifier> identifiers)
        {
            var qubits = Qubits.Where(q => identifiers.Contains(q.Identifier)).ToList(); 

            // TODO: This will fail if a qubit is used as parameter or guard
            // multiple times.
            if(qubits.Count != identifiers.Count())
            {
                throw new ArgumentException("Not all qubits were found in the graph");
            }

            return qubits;
        }

    }
}