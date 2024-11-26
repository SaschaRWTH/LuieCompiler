
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Optimization.Graphs.Interfaces;

namespace LUIECompiler.Optimization.Graphs.Nodes
{
    /// <summary>
    /// Represents a node in a graph.
    /// </summary>
    public abstract class Node : INode
    {
        /// <summary>
        /// Gets the input edges of the node.
        /// </summary>
        public virtual List<IEdge> InputEdges { get; } = [];

        /// <summary>
        /// Gets the output edges of the node.
        /// </summary>
        public virtual List<IEdge> OutputEdges { get; } = [];

        /// <summary>
        /// Gets the graph the node is in.
        /// </summary>
        public IGraph Graph { get; }

        public IEnumerable<INode> Predecessors
        {
            get
            {
                foreach (IEdge edge in InputEdges)
                {
                    yield return edge.Start;
                }
            }
        }

        public IEnumerable<INode> Successors
        {
            get
            {
                foreach (IEdge edge in OutputEdges)
                {
                    yield return edge.Start;
                }
            }
        }

        /// <summary>
        /// Creates a new node.
        /// </summary>
        /// <param name="graph"></param>
        public Node(IGraph graph)
        {
            Graph = graph;
        }

        /// <summary>
        /// Adds an input edge to the node.
        /// </summary>
        /// <param name="edge"></param>
        public virtual void AddInput(IEdge edge)
        {
            InputEdges.Add(edge);
        }

        /// <summary>
        /// Adds an output edge to the node.
        /// </summary>
        /// <param name="edge"></param>
        public virtual void AddOutput(IEdge edge)
        {
            OutputEdges.Add(edge);
        }

        /// <summary>
        /// Gets the input edge of the given qubit.
        /// </summary>
        /// <param name="qubit"></param>
        /// <returns></returns>
        public IEdge? GetInEdge(GraphQubit qubit)
        {
            return InputEdges.OfType<CircuitEdge>().FirstOrDefault(v => v.Qubit == qubit);
        }

        /// <summary>
        /// Gets the input edge of the given qubit.
        /// </summary>
        /// <param name="qubit"></param>
        /// <returns></returns>
        public INode GetPredecessor(GraphQubit qubit)
        {
            return GetInEdge(qubit)?.Start ?? throw new InternalException()
            {
                Reason = $"The does not exist a predecessor for the given qubit {qubit}",
            };
        }

        /// <summary>
        /// Gets the input edge of the given qubit.
        /// </summary>
        /// <param name="qubit"></param>
        /// <returns></returns>
        public IEdge GetOutEdge(GraphQubit qubit)
        {
            return OutputEdges.OfType<CircuitEdge>().FirstOrDefault(v => v.Qubit == qubit) ?? throw new InternalException()
            {
                Reason = $"No output edge found for qubit {qubit}."
            };
        }

        /// <summary>
        /// Gets the input edge of the given qubit.
        /// </summary>
        /// <param name="qubit"></param>
        /// <returns></returns>
        public INode GetSuccessor(GraphQubit qubit)
        {
            return GetOutEdge(qubit)?.End ?? throw new InternalException()
            {
                Reason = $"The does not exist a successor for the given qubit {qubit}",
            };
        }

        /// <summary>
        /// Gets the input edge of the given qubit.
        /// </summary>
        /// <param name="qubit"></param>
        /// <returns></returns>
        public IEdge? GetOutEdgeOrDefault(GraphQubit qubit)
        {
            return OutputEdges.OfType<CircuitEdge>().FirstOrDefault(v => v.Qubit == qubit);
        }
    }
}