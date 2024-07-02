namespace LUIECompiler.Common.Symbols
{

    public class Register : Symbol
    {
        /// <summary>
        /// The size of the register, i.e., the number of qubits it contains.
        /// </summary>
        public int Size { get; init; }
        public Register(string identifier, int size) : base(identifier) 
        {
            Size = size;
        }

        public override string ToString()
        {
            return $"Register: [Identifier = {Identifier}, size = {Size}]";
        }
    }

}