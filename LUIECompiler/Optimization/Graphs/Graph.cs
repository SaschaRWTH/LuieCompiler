using LUIECompiler.Optimization.Graphs.Interfaces;

namespace LUIECompiler.Optimization.Graphs
{
    public class Graph : IGraph
    {

        public List<INode> Nodes { get; } = [];
        public List<IVertex> Vertices { get; } = [];

        
        public void AddNode(INode node)
        {
            Nodes.Add(node);
        }

        public void RemoveNode(INode node)
        {
            Nodes.Remove(node);
        }

        public void AddVertex(IVertex vertex)
        {
            Vertices.Add(vertex);
        }
        

        public void RemoveVertex(IVertex vertex)
        {
            var start = vertex.Start;
            var end = vertex.End;

            start.OutputVertices.Remove(vertex);
            end.InputVertices.Remove(vertex);

            Vertices.Remove(vertex);
        }


        public void ReplacePath(Path old, Path replacement)
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

            foreach (IVertex vertex in old.Vertices)
            {
                RemoveVertex(vertex);
            }

            foreach (IVertex vertex in replacement.Vertices)
            {
                AddVertex(vertex);
            }
        }
    }
}