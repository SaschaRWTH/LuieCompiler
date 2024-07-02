using LUIECompiler.CodeGeneration.Definitions;

namespace LUIECompiler.CodeGeneration.Codes
{
    /// <summary>
    /// Represents a parameter of a gate.
    /// </summary>
    public class GateParameter
    {
        /// <summary>
        /// Register that the parameter is associated with.
        /// </summary>
        public required RegisterDefinition Register { get; init; }
    
        /// <summary>
        /// Return the code string representation of the parameter.
        /// </summary>
        /// <returns></returns>
        public virtual string ToParameterCode()
        {
            return Register.Identifier;
        } 
    }
}