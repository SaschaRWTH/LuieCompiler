using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration.Definitions
{
    /// <summary>
    /// Represents a unique identifier for a register or qubit.
    /// </summary>
    public class UniqueIdentifier
    {
        /// <summary>
        /// The unique identifier.
        /// </summary>
        public string Identifier { get; init; }

        public UniqueIdentifier(SymbolTable table)
        {
            Identifier = table.UniqueIdentifier;
        }
    }
}