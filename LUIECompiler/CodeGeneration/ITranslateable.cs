using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common.Symbols;
namespace LUIECompiler.CodeGeneration
{
    public interface ITranslateable
    {
        /// <summary>
        ///  Returns the QASM code for the statement.
        /// </summary>
        /// <returns></returns>
        public abstract QASMProgram ToQASM(List<Constant<int>> constants);
    }
}