namespace LUIECompiler.CodeGeneration.Exceptions
{ 
    /// <summary>
    /// An exception of internal error not caused by invalid source programs.
    /// </summary>
    public class InternalException : Exception
    {
        /// <summary>
        /// Reason for the internal Exception.
        /// </summary>
        public required string Reason { get; init; }
    }

}