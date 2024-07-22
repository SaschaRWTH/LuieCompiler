using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Gates
{
    public class DefinedGate : Gate
    {
        public CompositeGate CompositeGate { get; init; }

        public DefinedGate(CompositeGate compositeGate)
        {
            CompositeGate = compositeGate;
            NumberOfArguments = CompositeGate.Parameters.Count;
        }

        public override string GenerateCode(string parameters, List<GuardCode> negativeGuards, List<GuardCode> positiveGuards)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            string parameters = string.Join(", ", CompositeGate.Parameters.Select(p => p.Identifier));

            return $"Defined gate = \n\r {{ \n\r\t Identifier = {CompositeGate.Identifier} \n\r\t Parameter = {parameters}\n\r  }}";
        }
    }
}