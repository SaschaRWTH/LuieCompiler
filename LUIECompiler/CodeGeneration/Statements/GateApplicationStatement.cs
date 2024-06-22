
using System.Diagnostics.CodeAnalysis;
using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration.Statements
{
    public abstract class GateApplicationStatement : AbstractStatement
    {
        public required Gate Gate { get; init; }
        public required RegisterInfo Register { get; init; }

        public GateApplicationStatement(Gate gate, RegisterInfo register) 
        {
            Gate = gate;
            Register = register;
        }

        public override string ToQASM()
        {
            return $"{Gate.ToQASM()} {GetIdentifier(Register)};";
        }
    }

}