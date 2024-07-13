namespace LUIECompiler.Common.Errors
{
    public class TypeError : CompilationError
    {
        /// <summary>
        /// Identifier with the wrong type.
        /// </summary>
        public string Identifier { get; init; }

        /// <summary>
        /// The required type.
        /// </summary>
        public Type RequiredType { get; init; }

        /// <summary>
        /// The given type.
        /// </summary>
        public Type GivenType { get; init; }

        public TypeError(ErrorContext context, string identifier, Type requiredType, Type givenType)
        {
            Type = ErrorType.Critical;
            ErrorContext = context;
            Identifier = identifier;
            RequiredType = requiredType;
            GivenType = givenType;
            Description = $"The identifier {identifier} was of the wrong type. Expected {requiredType} but got {givenType}.";
        }
    }

}
