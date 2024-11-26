using LUIECompiler.Optimization.Graphs.Interfaces;

namespace LUIECompiler.Optimization.Graphs
{
    /// <summary>
    /// Represents a graph.
    /// </summary>
    public class Graph : IGraph
    {
        public List<INode> Nodes { get; } = [];
        public List<IEdge> Edges { get; } = [];

        
        public void AddNode(INode node)
        {
            Nodes.Add(node);
        }

        public void RemoveNode(INode node)
        {
            Nodes.Remove(node);
        }

        public void AddEdge(IEdge edge)
        {
            Edges.Add(edge);
        }
        

        public void RemoveEdge(IEdge edge)
        {
            var start = edge.Start;
            var end = edge.End;

            start.OutputEdges.Remove(edge);
            end.InputEdges.Remove(edge);

            Edges.Remove(edge);
        }


        public void ReplacePath(WirePath old, WirePath replacement)
        {
            if (old.Start != replacement.Start || old.End != replacement.End)
            {
                throw new ArgumentException($"Old path start(={old.Start})/end(={old.End}) nodes do not match replacement start(={replacement.Start})/end(={replacement.End}) nodes");
            }

            foreach (INode node in old.InnerNodes)
            {
                RemoveNode(node);
            }

            foreach (INode node in replacement.InnerNodes)
            {
                AddNode(node);
            }

            foreach (IEdge edge in old.Edges)
            {
                RemoveEdge(edge);
            }

            foreach (IEdge edge in replacement.Edges)
            {
                AddEdge(edge);
            }
        }
    }
}