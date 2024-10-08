
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
        /// Gets the input vertices of the node.
        /// </summary>
        public virtual List<IVertex> InputVertices { get; } = [];

        /// <summary>
        /// Gets the output vertices of the node.
        /// </summary>
        public virtual List<IVertex> OutputVertices { get; } = [];

        /// <summary>
        /// Gets the graph the node is in.
        /// </summary>
        public IGraph Graph { get; }

        public IEnumerable<INode> Predecessors
        {
            get
            {
                foreach (IVertex vertex in InputVertices)
                {
                    yield return vertex.Start;
                }
            }
        }

        public IEnumerable<INode> Successors
        {
            get
            {
                foreach (IVertex vertex in OutputVertices)
                {
                    yield return vertex.Start;
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
        /// Adds an input vertex to the node.
        /// </summary>
        /// <param name="vertex"></param>
        public virtual void AddInput(IVertex vertex)
        {
            InputVertices.Add(vertex);
        }

        /// <summary>
        /// Adds an output vertex to the node.
        /// </summary>
        /// <param name="vertex"></param>
        public virtual void AddOutput(IVertex vertex)
        {
            OutputVertices.Add(vertex);
        }

        /// <summary>
        /// Gets the input vertex of the given qubit.
        /// </summary>
        /// <param name="qubit"></param>
        /// <returns></returns>
        public IVertex? GetInVertex(GraphQubit qubit)
        {
            return InputVertices.OfType<CircuitVertex>().FirstOrDefault(v => v.Qubit == qubit);
        }

        /// <summary>
        /// Gets the input vertex of the given qubit.
        /// </summary>
        /// <param name="qubit"></param>
        /// <returns></returns>
        public INode GetPredecessor(GraphQubit qubit)
        {
            return GetInVertex(qubit)?.Start ?? throw new InternalException()
            {
                Reason = $"The does not exist a predecessor for the given qubit {qubit}",
            };
        }

        /// <summary>
        /// Gets the input vertex of the given qubit.
        /// </summary>
        /// <param name="qubit"></param>
        /// <returns></returns>
        public IVertex GetOutVertex(GraphQubit qubit)
        {
            return OutputVertices.OfType<CircuitVertex>().FirstOrDefault(v => v.Qubit == qubit) ?? throw new InternalException()
            {
                Reason = $"No output vertex found for qubit {qubit}."
            };
        }

        /// <summary>
        /// Gets the input vertex of the given qubit.
        /// </summary>
        /// <param name="qubit"></param>
        /// <returns></returns>
        public INode GetSuccessor(GraphQubit qubit)
        {
            return GetOutVertex(qubit)?.End ?? throw new InternalException()
            {
                Reason = $"The does not exist a successor for the given qubit {qubit}",
            };
        }

        /// <summary>
        /// Gets the input vertex of the given qubit.
        /// </summary>
        /// <param name="qubit"></param>
        /// <returns></returns>
        public IVertex? GetOutVertexOrDefault(GraphQubit qubit)
        {
            return OutputVertices.OfType<CircuitVertex>().FirstOrDefault(v => v.Qubit == qubit);
        }
    }
}