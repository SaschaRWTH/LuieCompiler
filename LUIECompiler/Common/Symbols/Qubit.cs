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

        /// <summary>
        /// Converts the qubit to a QASM parameter.
        /// </summary>
        /// <param name="definition"></param>
        /// <returns> The QASM parameter. </returns>
        public virtual QubitCode ToQASMCode(RegisterDefinition definition, List<Constant<int>> constants, int line)
        {
            return new()
            {
                Register = definition,
            };
        }
    }

}