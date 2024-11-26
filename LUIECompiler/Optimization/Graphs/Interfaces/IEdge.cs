namespace LUIECompiler.Optimization.Graphs.Interfaces
{
    /// <summary>
    /// Represents a edge in a graph.
    /// </summary>
    public interface IEdge
    {
        /// <summary>
        /// Gets the start node of the edge.
        /// </summary>
        public INode Start { get; }

        /// <summary>
        /// Gets the end node of the edge.
        /// </summary>
        public INode End { get; }

        /// <summary>
        /// Gets the graph the edge is in.
        /// </summary>
        public IGraph Graph { get; }
    }
}