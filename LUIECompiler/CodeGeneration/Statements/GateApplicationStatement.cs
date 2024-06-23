
using System.Diagnostics.CodeAnalysis;
using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration.Statements
{
    public class GateApplicationStatement : AbstractStatement
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
            return new($"{Gate} {GetIdentifier(Register)};");
        }
    }

}