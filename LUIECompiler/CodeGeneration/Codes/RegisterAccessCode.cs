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

        /// <summary>
        /// Unique identifier of the register.
        /// </summary>
        /// <returns></returns>

        public override string ToCode()
        {
            return $"{Identifier.Identifier}[{Index}]";
        }

        public override bool SemanticallyEqual(Code code)
        {
            if (code is not RegisterAccessCode regAccess)
            {
                return false;
            }
            return base.SemanticallyEqual(code) && regAccess.Index == Index;
        }
    }
}