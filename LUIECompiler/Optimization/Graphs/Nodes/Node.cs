
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

    }
}