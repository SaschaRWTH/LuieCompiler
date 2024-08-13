using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Optimization.Graphs.Nodes;
using LUIECompiler.Optimization.Rules;

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
                CreateQubits(definition);
            }
        }

        /// <summary>
        /// Creates the qubits of the graph.
        /// </summary>
        /// <param name="definition"></param>
        public void CreateQubits(DefinitionCode definition)
        {
            if (definition.Size == 1)
            {
                GraphQubit qubit = new(this, definition.Identifier);
                Qubits.Add(qubit);
                return;
            }

            for (int i = 0; i < definition.Size; i++)
            {
                GraphRegisterAccess qubit = new(this, definition.Identifier, i);
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
                HashSet<QubitCode> qubits = GetQubitsFromGate(gate);
                List<GraphQubit> graphQubits = qubits.Select(GraphQubitFromQubitCode).ToList();
                new GateNode(this, gate, graphQubits);
            }
        }

        /// <summary>
        /// Gets the identifiers used by the given <paramref name="gate"/>.
        /// </summary>
        /// <param name="gate"></param>
        /// <returns></returns>
        public HashSet<QubitCode> GetQubitsFromGate(GateApplicationCode gate)
        {
            // Use a hashset to avoid duplicates
            HashSet<QubitCode> identifiers = new();
            foreach (var guard in gate.Guards)
            {
                identifiers.Add(guard.Qubit);
            }
            foreach (var parameter in gate.Parameters)
            {
                identifiers.Add(parameter);
            }
            return identifiers;
        }

        /// <summary>
        /// Gets the graph qubits used in the gate <paramref name="code"/>.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public IEnumerable<GraphQubit> GraphQubitFromGateCode(GateApplicationCode code)
        {
            return GetQubitsFromGate(code).Select(q => GraphQubitFromQubitCode(q));
        }

        /// <summary>
        /// Gets the graph qubit from the given <paramref name="code"/>.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public GraphQubit GraphQubitFromQubitCode(QubitCode code)
        {
            if (code is RegisterAccessCode registerAccessCode)
            {
                return GetGraphQubit(code.Identifier.Identifier, registerAccessCode.Index);
            }

            return GetGraphQubit(code.Identifier.Identifier);
        }

        /// <summary>
        /// Gets the graph qubit with the given <paramref name="identifier"/>.
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public GraphQubit GetGraphQubit(string identifier, int index = -1)
        {
            if (index != -1)
            {
                return Qubits.OfType<GraphRegisterAccess>().Single(
                    q => q.Identifier.Identifier == identifier
                    && q.Index == index);
            }
            return Qubits.Single(q => q.Identifier.Identifier == identifier);
        }

        /// <summary>
        /// Applies the given <paramref name="rules"/> to the graph.
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="maxDepth"></param>
        public void ApplyOptimizationRules(IEnumerable<OptimizationRule> rules, int maxDepth)
        {
            foreach(GraphQubit qubit in Qubits)
            {
                ApplyOptimizationRules(rules, maxDepth, qubit);
            }
        }

        /// <summary>
        /// Applies the given <paramref name="rules"/> to the given <paramref name="qubit"/>.
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="maxDepth"></param>
        /// <param name="qubit"></param>
        public void ApplyOptimizationRules(IEnumerable<OptimizationRule> rules, int maxDepth, GraphQubit qubit)
        {
            WirePath path = new WirePath(qubit, qubit.Start, qubit.End);
            foreach (WirePath subpath in path.GetSubPaths(maxDepth))
            {
                subpath.ApplyOptimizationRules(rules);
            }
        }

        public QASMProgram ToQASM()
        {
            throw new NotImplementedException();
        }
    }
}