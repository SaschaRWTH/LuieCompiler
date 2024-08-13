using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Symbols;
using LUIECompiler.Optimization.Graphs.Interfaces;
using LUIECompiler.Optimization.Rules;

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

        public int Length => Nodes.Count;

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
        /// The qubit of the wire the path is part of.
        /// </summary>
        public GraphQubit Qubit { get; }

        /// <summary>
        /// Creates a new path. Uses the wire of the qubit and the start and end nodes.
        /// </summary>
        /// <param name="qubit"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public WirePath(GraphQubit qubit, INode start, INode end)
        {
            Qubit = qubit;

            IVertex current = GetQubitVertex(qubit, start.OutputVertices);
            Vertices.Add(current);
            while(current.End != end)
            {
                current = GetQubitVertex(qubit, current.End.OutputVertices);
                Vertices.Add(current);
            }
        }

        /// <summary>
        /// Creates a new path with maximum length <paramref name="maxLength"/>. Uses the wire of the qubit and the start and end nodes.
        /// </summary>
        /// <param name="qubit"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="maxLength">Maximum length of the path. A path needs to be at least 2 long.</param>
        public WirePath(GraphQubit qubit, INode start, INode end, int maxLength)
        {
            Qubit = qubit;

            IVertex current = GetQubitVertex(qubit, start.OutputVertices);
            Vertices.Add(current);
            int length = 2;
            while(current.End != end || Length < maxLength)
            {
                current = GetQubitVertex(qubit, current.End.OutputVertices);
                Vertices.Add(current);
                length++;
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

            if (Vertices[0] is not CircuitVertex startVertex)
            {
                throw new ArgumentException("The path does not start with a qubit vertex.");
            }
            Qubit = startVertex.Qubit;

            foreach(IVertex vertex in Vertices)
            {
                if (vertex is not CircuitVertex qubitVertex)
                {
                    throw new ArgumentException("The path contains a vertex that is not a qubit vertex.");
                }

                if (Qubit != qubitVertex.Qubit)
                {
                    throw new ArgumentException("The path contains vertices of different qubits.");
                }
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

        /// <summary>
        /// Gets all subpaths of the path upto a maximum length of <paramref name="maxLength"/>.
        /// </summary>
        /// <param name="qubit"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public IEnumerable<IPath> GetSubPaths(int maxLength)
        {
            foreach(INode node in Nodes)
            {
                for(int length = 2; length <= maxLength; length++)
                {
                    yield return new WirePath(Qubit, node, End, length);
                }
            }
        }

        public void ApplyOptimizationRules(IEnumerable<OptimizationRule> rules)
        {
            foreach (OptimizationRule rule in rules)
            {
                if (rule.IsApplicable(this))
                {
                    rule.Apply(this);
                }
            }
        }
    }
}