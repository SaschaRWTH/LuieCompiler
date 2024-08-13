using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Optimization.Graphs.Interfaces;

namespace LUIECompiler.Optimization.Graphs
{
    /// <summary>
    /// Represents a path on a qubit wire.
    /// </summary>
    public class WirePath : IPath
    {
        public List<IVertex> Vertices { get; } = [];

        public List<INode> Nodes =>
        [
            Start,
            .. InnerNodes,
            End
        ];

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
        /// Creates a new path. Uses the wire of the qubit and the start and end nodes.
        /// </summary>
        /// <param name="qubit"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public WirePath(GraphQubit qubit, INode start, INode end)
        {
            IVertex current = GetQubitVertex(qubit, start.OutputVertices);
            Vertices.Add(current);
            while(current.End != end)
            {
                current = GetQubitVertex(qubit, current.End.OutputVertices);
                Vertices.Add(current);
            }
        }

        /// <summary>
        /// Gets the vertex of the qubit in the given enumerable vertices.
        /// </summary>
        /// <param name="qubit"></param>
        /// <param name="vertices"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public IVertex GetQubitVertex(GraphQubit qubit, IEnumerable<IVertex> vertices)
        {
            return vertices.FirstOrDefault(v => v is CircuitVertex qubitVertex && qubitVertex.Qubit == qubit) ??
                   throw new ArgumentException("Enumerable does not contain a vertex of the qubit.");
        }

        /// <summary>
        /// Creates a new path.
        /// </summary>
        /// <param name="vertices"></param>
        /// <exception cref="ArgumentException"></exception>
        public WirePath(IEnumerable<IVertex> vertices)
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