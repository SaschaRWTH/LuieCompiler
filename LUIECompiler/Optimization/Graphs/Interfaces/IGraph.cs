namespace LUIECompiler.Optimization.Graphs.Interfaces
{
    /// <summary>
    /// Interface for a directed graph.
    /// </summary>
    public interface IGraph
    {
        /// <summary>
        /// Gets all nodes in the graph.
        /// </summary>
        public List<INode> Nodes { get; }

        /// <summary>
        /// Gets all edges in the graph.
        /// </summary>
        public List<IEdge> Edges { get; }

        /// <summary>
        /// Adds a node to the graph.
        /// </summary>
        /// <param name="node"></param>
        public void AddNode(INode node);

        /// <summary>
        /// Removes a node from the graph.
        /// </summary>
        /// <param name="node"></param>
        public void RemoveNode(INode node);

        /// <summary>
        /// Adds a edge to the graph.
        /// </summary>
        /// <param name="edge"></param>
        public void AddEdge(IEdge edge);

        /// <summary>
        /// Removes a edge from the graph.
        /// </summary>
        /// <param name="edge"></param>
        public void RemoveEdge(IEdge edge);

        /// <summary>
        /// Replaces a path in the graph.
        /// </summary>
        /// <param name="old"></param>
        /// <param name="replacement"></param>
        public void ReplacePath(WirePath old, WirePath replacement);
    }
}