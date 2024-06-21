
using System.Diagnostics.CodeAnalysis;
using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration.Statements
{
    public abstract class GateApplicationStatement : AbstractStatement
    {
        public Gate Gate { get; init; }

        public GateApplicationStatement(Gate gate) 
        {
            Gate = gate;
        }

        public override string ToQASM()
        {
            throw new NotImplementedException();
        }
    }

}