namespace LUIECompiler.Common.Symbols
{

    public class Register : Symbol
    {
        public int Size { get; init; }
        public Register(string identifier, int size) : base(identifier) 
        {
            Size = size;
        }
    }

}