namespace LUIECompiler.CodeGeneration.Codes
{
    /// <summary>
    /// Represents a parameter of a gate which is an access to a register.
    /// </summary>
    public class RegisterAccessGateParameter : GateParameter
    {
        /// <summary>
        /// Index in the register to access.
        /// </summary>
        public required int Index { get; init; }
    
        public override string ToParameterCode()
        {
            return $"{Register.Identifier}[{Index}]";
        } 
    }
}