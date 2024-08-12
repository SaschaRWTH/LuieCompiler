namespace LUIECompiler.Optimization.Graphs.Interfaces
{
    /// <summary>
    /// Represents a path in a graph.
    /// </summary>
    public interface IPath
    {
        /// <summary>
        /// Gets the vertices transversed by the path.
        /// </summary>
        public List<IVertex> Vertices { get; }

        /// <summary>
        /// Gets the nodes transversed by the path, excluding the start and end nodes.
        /// </summary>
        public IEnumerable<INode> InnerNodes { get; }

        /// <summary>
        /// Gets the start node of the path.
        /// </summary>
        public INode Start { get; }   

        /// <summary>
        /// Gets the end node of the path.
        /// </summary>
        public INode End { get; }   
    }
}