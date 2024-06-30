namespace LUIECompiler.Common.Symbols
{

    public class RegisterAccess : Qubit
    {
        public int Index { get; init; }

        public RegisterAccess(string identifier, int index) : base(identifier: identifier) 
        {
            Index = index;
        }
    }

}