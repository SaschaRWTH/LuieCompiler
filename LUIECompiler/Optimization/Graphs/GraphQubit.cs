using LUIECompiler.CodeGeneration.Definitions;
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
            graph.AddNode(Start);
            graph.AddNode(End);

            IVertex vertex = new CircuitVertex(graph, this, Start, End);
            graph.AddVertex(vertex);
        }

        /// <summary>
        /// Adds a gate node to the qubit path.
        /// </summary>
        /// <param name="node"></param>
        /// <exception cref="InternalException"></exception>
        public void AddGateNode(GateNode node)
        {
            IVertex last = End.InputVertex ?? throw new InternalException   
            {
                Reason = "Output node must have exactly one output vertex",
            };

            IVertex input = new CircuitVertex(Graph, this, last.Start, node);
            IVertex output = new CircuitVertex(Graph, this, node, last.End);

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
        public IEnumerable<IVertex> TracePath()
        {
            IVertex current = Start.OutputVertex ?? throw new InternalException
            {
                Reason = "Input node must have exactly one output vertex",
            };
            yield return current;


            while (current.End is not OutputNode)
            {
                INode nextNode = current.End;

                current = nextNode.OutputVertices.OfType<CircuitVertex>().SingleOrDefault(v => v.Qubit == this) ??
                    throw new InternalException
                    {
                        Reason = "Output node must have exactly one output vertex",
                    };


                yield return current;
            }
        }
    }
}