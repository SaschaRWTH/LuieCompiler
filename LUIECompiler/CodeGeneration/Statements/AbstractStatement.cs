
namespace LUIECompiler.CodeGeneration.Statements
{
    // TODO: Turn into interface?
    public abstract class AbstractStatement 
    {

        /// <summary>
        ///  Returns the QASM code for the statement.
        /// </summary>
        /// <returns></returns>
        public abstract string ToQASM();
    }

}