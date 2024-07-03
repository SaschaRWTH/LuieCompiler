namespace LUIECompiler.CodeGeneration.Codes
{
    /// <summary>
    /// Represents a parameter of a gate which is an access to a register.
    /// </summary>
    public class RegisterAccessCode : QubitCode
    {
        /// <summary>
        /// Index in the register to access.
        /// </summary>
        public required int Index { get; init; }
    
        public override string ToCode()
        {
            return $"{Register.Identifier}[{Index}]";
        } 
    }
}