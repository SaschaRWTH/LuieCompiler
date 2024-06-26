namespace LUIECompiler.Common.Symbols
{

    public abstract class Symbol
    {
        /// <summary>
        /// Identifier of the symbol.
        /// </summary>
        public string Identifier { get; init; }

        public Symbol(string identifier)
        {
            Identifier = identifier;
        }
    }

}