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
    }

}