using LUIECompiler.CodeGeneration.Declarations;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Optimization.Graphs.Interfaces;
using LUIECompiler.Optimization.Graphs.Nodes;

namespace LUIECompiler.Optimization.Graphs
{
    /// <summary>
    /// Represents a qubit in a quantum circuit graph.
    /// </summary>
    public class GraphQubit
    {
        /// <summary>
        /// The graph the qubit is in.
        /// </summary>
        public IGraph Graph { get; }

        /// <summary>
        /// The start node of the qubit.
        /// </summary>
        public InputNode Start { get; }

        /// <summary>
        /// The end node of the qubit.
        /// </summary>
        public OutputNode End { get; }

        /// <summary>
        /// The identifier of the qubit.
        /// </summary>
        public UniqueIdentifier Identifier { get; }

        /// <summary>
        /// Indicates whether the qubit can be removed from the graph.
        /// A qubit can be removed if no gates are applied to it.
        /// </summary>
        public virtual bool CanBeRemoved
        {
            get
            {
                IEdge edge = Start.OutputEdge ?? throw new InternalException
                {
                    Reason = "Input node must have exactly one output edge",
                };

                return edge.Start == Start && edge.End == End;
            }
        }

        /// <summary>
        /// Creates a new qubit.
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="identifier"></param>
        public GraphQubit(CircuitGraph graph, UniqueIdentifier identifier)
        {
            Graph = graph;
            Identifier = identifier;

            Start = new(graph);
            End = new(graph);
        }

        /// <summary>
        /// Creates and add input and output nodes for the graph qubit.
        /// </summary>
        /// <exception cref="InternalException"></exception>
        public void AddIONodes()
        {
            Graph.AddNode(Start);
            Graph.AddNode(End);

            IEdge edge = new CircuitEdge(Graph, this, Start, End);
            Graph.AddEdge(edge);
        }

        /// <summary>
        /// Adds a gate node to the qubit path.
        /// </summary>
        /// <param name="node"></param>
        /// <exception cref="InternalException"></exception>
        public void AddGateNode(GateNode node)
        {
            IEdge last = End.InputEdge ?? throw new InternalException
            {
                Reason = "Output node must have exactly one output edge",
            };

            IEdge input = new CircuitEdge(Graph, this, last.Start, node);
            IEdge output = new CircuitEdge(Graph, this, node, last.End);

            Graph.ReplacePath(new([last]), new([input, output]));
        }

        /// <summary>
        /// Gets the path of the qubit.
        /// </summary>
        /// <returns></returns>
        public WirePath GetPath()
        {
            return new(TracePath());
        }

        /// <summary>
        /// Traces the path of the qubit.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InternalException"></exception>
        public IEnumerable<IEdge> TracePath()
        {
            IEdge current = Start.OutputEdge ?? throw new InternalException
            {
                Reason = "Input node must have exactly one output edge",
            };
            yield return current;


            while (current.End is not OutputNode)
            {
                INode nextNode = current.End;

                current = nextNode.OutputEdges.OfType<CircuitEdge>().SingleOrDefault(v => v.Qubit == this) ??
                    throw new InternalException
                    {
                        Reason = "Output node must have exactly one output edge",
                    };


                yield return current;
            }
        }

        /// <summary>
        /// Removes the qubit from the graph if possible.
        /// </summary>
        /// <exception cref="InternalException"></exception>
        public void Remove()
        {
            if (!CanBeRemoved)
            {
                return;
            }

            IEdge edge = Start.OutputEdge ?? throw new InternalException
            {
                Reason = "Input node must have exactly one output edge",
            };

            if (Graph is not CircuitGraph circuitGraph)
            {
                throw new InternalException
                {
                    Reason = "Graph must be a circuit graph",
                };
            }

            circuitGraph.RemoveEdge(edge);

            circuitGraph.RemoveNode(Start);
            circuitGraph.RemoveNode(End);

            circuitGraph.Qubits.Remove(this);
        }

        public override string ToString()
        {
            return $"GraphQubit = {{ Identifier = {Identifier} }}";
        }
    }
}