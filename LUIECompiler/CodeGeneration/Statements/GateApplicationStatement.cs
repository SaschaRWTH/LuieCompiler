
using System.Diagnostics.CodeAnalysis;
using LUIECompiler.Common;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.CodeGeneration.Definitions;

namespace LUIECompiler.CodeGeneration.Statements
{
    public class GateApplicationStatement : Statement
    {
        public required Gate Gate { get; init; }
        public required RegisterInfo Register { get; init; }

        public GateApplicationStatement() {}

        public GateApplicationStatement(Gate gate, RegisterInfo register) 
        {
            Gate = gate;
            Register = register;
        }

        public override QASMCode ToQASM()
        {
            return new(new GateCode(){
                Gate = Gate,
                Guards = [],
                Register = GetDefinition(Register) as RegisterDefinition ?? throw new Exception("Type error"),
            });
        }
    }

}