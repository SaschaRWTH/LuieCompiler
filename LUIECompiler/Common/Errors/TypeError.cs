namespace LUIECompiler.Common.Errors
{
    public class TypeError : CompilationError
    {
        /// <summary>
        /// Identifier with the wrong type.
        /// </summary>
        public string Identifier { get; init; }

        public Type RequiredType { get; init; }

        public Type GivenType { get; init; }

        public TypeError(int line, string identifier, Type requiredType, Type givenType)
        {
            Type = ErrorType.Critical;
            Line = line;
            Identifier = identifier;
            RequiredType = requiredType;
            GivenType = givenType;
            Description = $"The identifier {identifier} was of the wrong type. Expected {requiredType} but got {givenType}.";
        }
    }

}
