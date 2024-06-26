using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.CodeGeneration.Definitions;

namespace LUIECompiler.Common.Symbols
{

    public class Qubit : Register
    {
        public Qubit(string identifier) : base(identifier: identifier, size: 1)
        {
        }

        public override string ToString()
        {
            return $"Qubit: [Identifier = {Identifier}]";
        }

        public virtual GateParameter ToQASMParameter(RegisterDefinition definition)
        {
            return new()
            {
                Register = definition,
            };
        }
    }

}