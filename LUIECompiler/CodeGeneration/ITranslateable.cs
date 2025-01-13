using LUIECompiler.CodeGeneration.Codes;

namespace LUIECompiler.CodeGeneration
{
    /// <summary>
    /// Interface for classes that can be translated to QASM code.
    /// </summary>
    public interface ITranslatable
    {
        /// <summary>
        /// Returns the QASM code for the statement.
        /// </summary>
        /// <returns>The <see cref="QASMProgram"/> of the <see cref="ITranslatable"/>.</returns>
        public abstract QASMProgram ToQASM(CodeGenerationContext context);
    }
}