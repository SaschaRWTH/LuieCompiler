namespace LUIECompiler.Optimization.Graphs.Interfaces
{
    /// <summary>
    /// Interface for a node in a graph directed.
    /// </summary>
    public interface INode
    {
        /// <summary>
        /// Gets the input vertices of the node.
        /// </summary>
        public List<IVertex> InputVertices { get; }

        /// <summary>
        /// Gets the output vertices of the node.
        /// </summary>
        public List<IVertex> OutputVertices { get; }

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
        /// Adds an input vertex to the node.
        /// </summary>
        /// <param name="vertex"></param>
        public void AddInput(IVertex vertex);

        /// <summary>
        /// Adds an output vertex to the node.
        /// </summary>
        /// <param name="vertex"></param>
        public void AddOutput(IVertex vertex);
    }
}