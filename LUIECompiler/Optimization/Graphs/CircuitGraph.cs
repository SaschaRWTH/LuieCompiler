using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.CodeGeneration.Definitions;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Optimization.Graphs.Interfaces;
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
        /// Removes all unused qubits from the graph.
        /// </summary>
        public void RemoveUnusedQubits()
        {
            List<GraphQubit> unusedQubits = Qubits.Where(q => q.CanBeRemoved).ToList();
            foreach (GraphQubit qubit in unusedQubits)
            {
                qubit.Remove();
            }
        }

        /// <summary>
        /// Applies the given <paramref name="rules"/> to the graph.
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="maxDepth"></param>
        public void ApplyOptimizationRules(IEnumerable<OptimizationRule> rules, int maxDepth)
        {
            foreach (GraphQubit qubit in Qubits)
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

        /// <summary>
        /// Translates the graph to a QASM program.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InternalException"></exception>
        public QASMProgram ToQASM()
        {
            if (Qubits.Count == 0)
            {
                return new QASMProgram();
            }

            List<Code> code = [.. GetDefinitions()];

            Dictionary<GraphQubit, INode> qubitToNode = new();
            foreach (GraphQubit qubit in Qubits)
            {
                qubitToNode[qubit] = qubit.Start.OutputVertex?.End ?? throw new InternalException()
                {
                    Reason = $"Start node of qubit {qubit} does not have an output vertex."
                };
            }

            GraphQubit current = Qubits.First();
            // Continue loop while not all qubits are translated
            while (qubitToNode.Any(pair => pair.Key.End != pair.Value))
            {
                if (qubitToNode[current] is not GateNode gateNode)
                {
                    current = Qubits.FirstOrDefault(q => qubitToNode[q] is GateNode) ?? throw new InternalException()
                    {
                        Reason = "No remaining gate node found, but not all qubits at end nodes."
                    };
                    continue;
                }

                List<INode> predecessors = gateNode.Predecessors.ToList();
                // If gate only operates on one qubit, it can allways be translated 
                // (not dependent on other gates to have been executed on other wires)
                // This is technically a special case of the following, but it is easier to handle it separately
                if (predecessors.Count <= 1)
                {
                    code.Add(gateNode.GateCode);
                    qubitToNode[current] = gateNode.GetOutVertex(current).End;
                    continue;
                }
                IEnumerable<GraphQubit> qubits = gateNode.Qubits;

                // Checks that all predecessors are gate nodes
                bool continueLoop = false;
                foreach (GraphQubit qubit in qubits)
                {
                    if (qubitToNode[qubit] is not GateNode qubitNode)
                    {
                        throw new InternalException()
                        {
                            Reason = "Qubit node is not a gate node. It should not be possible to reach this state."
                        };
                    }

                    // If the qubit is not at the current node
                    if (qubitNode != gateNode)
                    {
                        current = qubit;
                        continueLoop = true;
                        break;
                    }
                }
                if (continueLoop)
                {
                    continue;
                }

                // Add gate to translation
                code.Add(gateNode.GateCode);

                // Move each qubit to the next node
                foreach (GraphQubit qubit in qubits)
                {
                    qubitToNode[qubit] = gateNode.GetOutVertex(qubit).End;
                }
            }
            return new QASMProgram(code);
        }

        /// <summary>
        /// Gets a list of definitions code of the graph.
        /// </summary>
        /// <returns></returns>
        public List<DefinitionCode> GetDefinitions()
        {
            Dictionary<UniqueIdentifier, IEnumerable<GraphQubit>> identifierMap = [];
            foreach (GraphQubit qubit in Qubits)
            {
                if (!identifierMap.ContainsKey(qubit.Identifier))
                {
                    identifierMap[qubit.Identifier] = [];
                }
                identifierMap[qubit.Identifier] = identifierMap[qubit.Identifier].Append(qubit);
            }

            List<DefinitionCode> definitions = [];

            foreach (var pair in identifierMap)
            {
                definitions.Add(new DefinitionCode()
                {
                    Identifier = pair.Key,
                    Size = pair.Value.Count()
                });
            }

            return definitions;
        }
    }
}