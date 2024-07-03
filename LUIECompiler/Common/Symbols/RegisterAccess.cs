using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.CodeGeneration.Definitions;

namespace LUIECompiler.Common.Symbols
{

    public class RegisterAccess : Qubit
    {
        /// <summary>
        /// Index of the qubit in the <see cref="Register"/>.
        /// </summary>
        public int Index { get; init; }

        /// <summary>
        /// Register that is accessed.
        /// </summary>
        public Register Register { get; init; }

        public RegisterAccess(Register register, int index) : base(identifier: register.Identifier) 
        {
            Index = index;
            Register = register;
        }

        public override QubitCode ToQASMParameter(RegisterDefinition definition)
        {
            return new RegisterAccessCode()
            {
                Register = definition,
                Index = Index,
            };
        }
    }

}