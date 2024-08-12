using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Optimization.Graphs.Interfaces;

namespace LUIECompiler.Optimization.Graphs
{
    public class Path : IPath
    {
        public List<IVertex> Vertices { get; } = [];

        public INode Start
        {
            get
            {
                if (Vertices.Count == 0)
                {
                    throw new InternalException()
                    {
                        Reason = "The path is empty."
                    };
                }

                return Vertices[0].Start;
            }
        }

        public INode End
        {
            get
            {
                if (Vertices.Count == 0)
                {
                    throw new InternalException()
                    {
                        Reason = "The path is empty."
                    };
                }

                return Vertices[^1].End;
            }
        }

        public IEnumerable<INode> InnerNodes
        {
            get
            {
                for (int i = 0; i < Vertices.Count - 1; i++)
                {
                    yield return Vertices[i].End;
                }
            }
        }

        public Path(IEnumerable<IVertex> vertices)
        {
            Vertices.AddRange(vertices);

            if (Vertices.Count == 0)
            {
                throw new ArgumentException("The path is empty.");
            }

            if (!IsUnInterrupted())
            {
                throw new ArgumentException("The path is interrupted.");
            }
        }

        public bool IsUnInterrupted()
        {
            for (int i = 0; i < Vertices.Count - 1; i++)
            {
                if (Vertices[i].End != Vertices[i + 1].Start)
                {
                    return false;
                }
            }
            return true;
        }

        public void AddVertex(IVertex vertex)
        {
            Vertices.Add(vertex);
        }
    }
}