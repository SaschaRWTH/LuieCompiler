
namespace LUIECompiler.CodeGeneration.Codes
{
    public abstract class Code
    {
        /// <summary>
        /// Prints the code as a string.
        /// </summary>
        /// <returns></returns>
        public abstract string ToCode();

        // TODO: Add test cases for semantic equality
        /// <summary>
        /// Indicates whether the code is semantically equal to another <paramref name="code"/>.
        /// </summary>
        /// <returns></returns>
        public abstract bool SemanticallyEqual(Code code);
    }
}