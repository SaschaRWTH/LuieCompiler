namespace LUIECompiler.Optimization.Graphs.Interfaces
{
    /// <summary>
    /// Represents a path in a graph.
    /// </summary>
    public interface IPath
    {
        /// <summary>
        /// Gets the nodes transversed by the path.
        /// </summary>
        public List<INode> Nodes { get; }
        
        /// <summary>
        /// Gets the vertices transversed by the path.
        /// </summary>
        public List<IVertex> Vertices { get; }

        /// <summary>
        /// Gets the length of the path.
        /// </summary>
        public int Length { get; }

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

        /// <summary>
        /// Gets all subpaths of the path with a maximum length of <paramref name="maxLength"/>.
        /// </summary>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public IEnumerable<IPath> GetSubPaths(int maxLength);
    }
}