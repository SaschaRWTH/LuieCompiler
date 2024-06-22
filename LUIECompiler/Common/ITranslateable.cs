namespace LUIECompiler.Common
{
    public interface ITranslateable
    {
        /// <summary>
        ///  Returns the QASM code for the statement.
        /// </summary>
        /// <returns></returns>
        public abstract string ToQASM();
    }
}