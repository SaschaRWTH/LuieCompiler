using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Gates
{
    public class CustomGate : Gate
    {
        public CompositeGate CompositeGate { get; init; }

        public CustomGate(CompositeGate compositeGate)
        {
            CompositeGate = compositeGate;
            NumberOfArguments = CompositeGate.Parameters.Count;
        }

        public override string GenerateCode(string parameters, List<GuardCode> negativeGuards, List<GuardCode> positiveGuards)
        {
            throw new NotImplementedException();
        }
    }
}