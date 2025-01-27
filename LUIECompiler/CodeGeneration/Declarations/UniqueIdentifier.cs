using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration.Declarations
{
    /// <summary>
    /// Represents a unique identifier for a register or qubit.
    /// </summary>
    public class UniqueIdentifier
    {
        /// <summary>
        /// The unique identifier.
        /// </summary>
        public string Identifier { get; }

        public UniqueIdentifier(string identifier)
        {
            Identifier = identifier;
        }

        public UniqueIdentifier(SymbolTable table)
        {
            Identifier = table.UniqueIdentifier;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not UniqueIdentifier identifier)
            {
                return false;
            }
            return Identifier == identifier.Identifier;
        }

        public override int GetHashCode()
        {
            return Identifier.GetHashCode();
        }

        public override string ToString()
        {
            return Identifier;
        }
    }
}