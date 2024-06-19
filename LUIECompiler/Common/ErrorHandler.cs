
using LUIECompiler.Common.Errors;

namespace LUIECompiler.Common
{
    public class ErrorHandler
    {
        /// <summary>
        /// List of all compilation errors.
        /// </summary>
        public List<CompilationError> Errors { get; init; } = new();

        /// <summary>
        /// Gets a list of all critical errors.
        /// </summary>
        public List<CompilationError> CriticalErrors { get => Errors.Where(e => e.Type == ErrorType.Critical).ToList(); }

        /// <summary>
        /// Gets a list of all warnings.
        /// </summary>
        public List<CompilationError> Warnings { get => Errors.Where(e => e.Type == ErrorType.Warning).ToList(); }

        /// <summary>
        /// Indicates if the error handler contains critical errors.
        /// </summary>
        public bool ContainsCriticalError { get => CriticalErrors.Count > 0; }

        /// <summary>
        /// Saves a given <paramref name="error"/> to the error handler.
        /// </summary>
        /// <param name="error"></param>
        public void Report(CompilationError error)
        {
            Errors.Add(error);
        }

        public override string ToString()
        {
            if (Errors.Count == 0)
            {
                return "No errors found";
            }

            string text = "";

            foreach (var error in CriticalErrors)
            {
                text += $"{error} \n";
            }
            foreach (var error in Warnings)
            {
                text += $"{error} \n";
            }

            return text;
        }
    }


}
