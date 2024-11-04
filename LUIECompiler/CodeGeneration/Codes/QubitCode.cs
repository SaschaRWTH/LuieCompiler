using LUIECompiler.CodeGeneration.Declarations;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Codes
{
    /// <summary>
    /// Represents a parameter of a gate.
    /// </summary>
    public class QubitCode : Code
    {
        /// <summary>
        /// Register that the parameter is associated with.
        /// </summary>
        public required RegisterDeclaration Register { get; init; }

        /// <summary>
        /// Unique identifier of the qubit.
        /// </summary>
        public required UniqueIdentifier Identifier { get; init; }
    
        /// <summary>
        /// Return the code string representation of the parameter.
        /// </summary>
        /// <returns></returns>
        public override string ToCode()
        {
            return Identifier.Identifier;
        } 

        public override bool SemanticallyEqual(Code code)
        {
            if (code is not QubitCode qubitCode)
            {
                return false;
            }

            return Identifier == qubitCode.Identifier;
        }
    }
}