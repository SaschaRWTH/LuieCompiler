using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.Common.Errors
{
    public class InvalidArguments : CompilationError
    {
        /// <summary>
        /// Gate with the wrong number of arguments.
        /// </summary>
        public Gate Gate { get; init; }

        /// <summary>
        /// Given number of arguments.
        /// </summary>
        public int NumberOfArguments { get; init; }
        
        public InvalidArguments(int line, Gate gate, int numberOfArguments)
        {
            Type = ErrorType.Critical;
            Line = line;
            Gate = gate;
            NumberOfArguments = numberOfArguments;
            Description = $"The gate '{Gate}' takes {Gate.NumberOfArguments} arguments, but received {NumberOfArguments}.";
        }
    }

}