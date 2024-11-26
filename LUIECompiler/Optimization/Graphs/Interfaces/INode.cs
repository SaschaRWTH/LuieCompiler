namespace LUIECompiler.Optimization.Graphs.Interfaces
{
    /// <summary>
    /// Interface for a node in a graph directed.
    /// </summary>
    public interface INode
    {
        /// <summary>
        /// Gets the input edges of the node.
        /// </summary>
        public List<IEdge> InputEdges { get; }

        /// <summary>
        /// Gets the output edges of the node.
        /// </summary>
        public List<IEdge> OutputEdges { get; }

        /// <summary>
        /// Gets the direct predecessors of the node.
        /// </summary>
        public IEnumerable<INode> Predecessors { get; }

        /// <summary>
        /// Gets the direct successors of the node.
        /// </summary>
        public IEnumerable<INode> Successors { get; }

        /// <summary>
        /// Gets the graph the node is in.
        /// </summary>
        public IGraph Graph { get; }

        /// <summary>
        /// Adds an input edge to the node.
        /// </summary>
        /// <param name="edge"></param>
        public void AddInput(IEdge edge);

        /// <summary>
        /// Adds an output edge to the node.
        /// </summary>
        /// <param name="edge"></param>
        public void AddOutput(IEdge edge);
    }
}