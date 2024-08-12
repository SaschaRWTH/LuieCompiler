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

        /// <summary>
        /// Creates a new path.
        /// </summary>
        /// <param name="vertices"></param>
        /// <exception cref="ArgumentException"></exception>
        public Path(IEnumerable<IVertex> vertices)
        {
            Vertices = vertices.ToList();      
            if (Vertices.Count == 0)
            {
                throw new ArgumentException("The path is empty.");
            }

            if (!IsUnInterrupted())
            {
                throw new ArgumentException("The path is interrupted.");
            }
        }

        /// <summary>
        /// Indicates whether the path is uninterrupted.
        /// </summary>
        /// <returns></returns>
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
    }
}