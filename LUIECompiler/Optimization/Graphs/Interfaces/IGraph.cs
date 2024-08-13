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
        /// Gets all vertices in the graph.
        /// </summary>
        public List<IVertex> Vertices { get; }

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
        /// Adds a vertex to the graph.
        /// </summary>
        /// <param name="vertex"></param>
        public void AddVertex(IVertex vertex);

        /// <summary>
        /// Removes a vertex from the graph.
        /// </summary>
        /// <param name="vertex"></param>
        public void RemoveVertex(IVertex vertex);

        /// <summary>
        /// Replaces a path in the graph.
        /// </summary>
        /// <param name="old"></param>
        /// <param name="replacement"></param>
        public void ReplacePath(WirePath old, WirePath replacement);
    }
}