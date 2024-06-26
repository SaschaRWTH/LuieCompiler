namespace LUIECompiler.Common.Symbols
{

    public class Symbol
    {
        public string Identifier { get; init; }

        public Symbol(string identifier)
        {
            Identifier = identifier;
        }
    }

}