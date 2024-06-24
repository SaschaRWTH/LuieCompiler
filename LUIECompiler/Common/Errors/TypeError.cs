namespace LUIECompiler.Common.Errors
{
    public class TypeError : CompilationError
    {
        /// <summary>
        /// Identifier with the wrong type.
        /// </summary>
        public string Identifier { get; init; }

        public TypeError(int line, string identifier)
        {
            Type = ErrorType.Critical;
            Line = line;
            Identifier = identifier;
            // TODO: Add expected and current type
            Description = $"The identifier {identifier} was of the wrong type.";
        }
    }

}
