
using LUIECompiler.Common;
using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.CodeGeneration.Statements
{
    public class QuantumElseStatement : QuantumIfStatement
    {
        public override QASMProgram ToQASM()
        {
            return Block.ToQASM().AddControl(identifier: IdentifierString(), negated: true);
        }
    }

}