using LUIECompiler.Optimization.Graphs.Nodes;

namespace LUIECompiler.Optimization.Graphs
{
    public interface IPath
    {
        public List<IVertex> Vertices { get; }
        public IEnumerable<INode> InnerNodes { get; }

        public INode Start { get; }   
        public INode End { get; }   
    }
}