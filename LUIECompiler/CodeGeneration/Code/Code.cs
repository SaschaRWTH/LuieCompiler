using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration.Codes
{
    public abstract class Code 
    {
        /// <summary>
        /// Prints the code as a string.
        /// </summary>
        /// <returns></returns>
        public abstract string ToCode(); 
    }
}