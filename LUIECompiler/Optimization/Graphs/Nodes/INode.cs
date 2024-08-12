namespace LUIECompiler.Optimization.Graphs.Nodes
{
    public interface INode
    {
        public List<IVertex> InputVertices { get; }
        public List<IVertex> OutputVertices { get; }

        public IGraph Graph { get; }

        public void AddInput(IVertex vertex);
        public void AddOutput(IVertex vertex);
    }
}