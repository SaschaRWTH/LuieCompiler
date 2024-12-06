using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Optimization.Graphs.Interfaces;

namespace LUIECompiler.Optimization.Graphs.Nodes
{
    public class CircuitNode : Node
    {
        public CircuitNode(CircuitGraph graph) : base(graph)
        {
        }

        /// <summary>
        /// Gets the input edge of the given qubit.
        /// </summary>
        /// <param name="qubit"></param>
        /// <returns></returns>
        public IEdge? GetInEdge(GraphQubit qubit)
        {
            return InputEdges.OfType<CircuitEdge>().FirstOrDefault(v => v.Qubit == qubit);
        }

        /// <summary>
        /// Gets the input edge of the given qubit.
        /// </summary>
        /// <param name="qubit"></param>
        /// <returns></returns>
        public INode GetPredecessor(GraphQubit qubit)
        {
            return GetInEdge(qubit)?.Start ?? throw new InternalException()
            {
                Reason = $"The does not exist a predecessor for the given qubit {qubit}",
            };
        }

        /// <summary>
        /// Gets the input edge of the given qubit.
        /// </summary>
        /// <param name="qubit"></param>
        /// <returns></returns>
        public IEdge GetOutEdge(GraphQubit qubit)
        {
            return OutputEdges.OfType<CircuitEdge>().FirstOrDefault(v => v.Qubit == qubit) ?? throw new InternalException()
            {
                Reason = $"No output edge found for qubit {qubit}."
            };
        }

        /// <summary>
        /// Gets the input edge of the given qubit.
        /// </summary>
        /// <param name="qubit"></param>
        /// <returns></returns>
        public INode GetSuccessor(GraphQubit qubit)
        {
            return GetOutEdge(qubit)?.End ?? throw new InternalException()
            {
                Reason = $"The does not exist a successor for the given qubit {qubit}",
            };
        }

        /// <summary>
        /// Gets the input edge of the given qubit.
        /// </summary>
        /// <param name="qubit"></param>
        /// <returns></returns>
        public IEdge? GetOutEdgeOrDefault(GraphQubit qubit)
        {
            return OutputEdges.OfType<CircuitEdge>().FirstOrDefault(v => v.Qubit == qubit);
        }
    }
}