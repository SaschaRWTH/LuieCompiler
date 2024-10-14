using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Optimization.Graphs.Nodes;

namespace LUIECompiler.Optimization.Graphs
{
    public class GraphGuard
    {
        public bool Negated { get; }

        public GraphQubit Qubit { get; }

        public GateNode Gate { get; }

        public GuardCode Code
        {
            get => FromGraphQubitToCode();
        }

        public GraphGuard(GraphQubit qubit, bool negated, GateNode gateNode)
        {
            Qubit = qubit;
            Negated = negated;
            Gate = gateNode;
        }


        private GuardCode FromGraphQubitToCode()
        {
            if (Qubit is GraphRegisterAccess access)
            {
                return FromGraphRegisterAccessToCode(access);
            }

            foreach (GuardCode guard in Gate.GateCode.Guards)
            {
                if (guard.Qubit.Identifier == Qubit.Identifier)
                {
                    return guard;
                }
            }

            throw new InternalException()
            {
                Reason = "The guard does not exist in the gate."
            };
        }
        private GuardCode FromGraphRegisterAccessToCode(GraphRegisterAccess access)
        {

            foreach (GuardCode guard in Gate.GateCode.Guards.Where(guard => guard.Qubit is RegisterAccessCode accessCode && accessCode.Index == access.Index))
            {
                if (guard.Qubit.Identifier == Qubit.Identifier)
                {
                    return guard;
                }
            }

            throw new InternalException()
            {
                Reason = "The guard does not exist in the gate."
            };
        }
    }
}