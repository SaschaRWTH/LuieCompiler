using LUIECompiler.CodeGeneration.Codes;
namespace LUIECompiler.CodeGeneration
{
    public interface ITranslateable
    {
        /// <summary>
        ///  Returns the QASM code for the statement.
        /// </summary>
        /// <returns></returns>
        public abstract QASMCode ToQASM();
    }
}