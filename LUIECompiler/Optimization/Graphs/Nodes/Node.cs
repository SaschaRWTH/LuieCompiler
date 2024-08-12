
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
        public IGraph Graph{ get; }

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
    }
}