using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.CodeGeneration.Definitions;

namespace LUIECompiler.Common.Symbols
{

    public class RegisterAccess : Qubit
    {
        public int Index { get; init; }
        public Register Register { get; init; }

        public RegisterAccess(Register register, int index) : base(identifier: register.Identifier) 
        {
            Index = index;
            Register = register;
        }

        public override GateParameter ToQASMParameter(RegisterDefinition definition)
        {
            return new RegisterAccessGateParameter()
            {
                Register = definition,
                Index = Index,
            };
        }
    }

}