namespace LUIECompiler.Common.Errors
{
    /// <summary>
    /// Represents an error where an identifier has the wrong type.
    /// </summary>
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

        /// <summary>
        /// Creates a new type error.
        /// </summary>
        /// <param name="context">Context where the identifier was used.</param>
        /// <param name="identifier">Identifier with the wrong type.</param>
        /// <param name="requiredType">Required type in the context.</param>
        /// <param name="givenType">Type of the identifier.</param>
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
