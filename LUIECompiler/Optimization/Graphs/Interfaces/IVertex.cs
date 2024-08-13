namespace LUIECompiler.Optimization.Graphs.Interfaces
{
    /// <summary>
    /// Represents a vertex in a graph.
    /// </summary>
    public interface IVertex
    {
        /// <summary>
        /// Gets the start node of the vertex.
        /// </summary>
        public INode Start { get; }

        /// <summary>
        /// Gets the end node of the vertex.
        /// </summary>
        public INode End { get; }

        /// <summary>
        /// Gets the graph the vertex is in.
        /// </summary>
        public IGraph Graph { get; }
    }
}