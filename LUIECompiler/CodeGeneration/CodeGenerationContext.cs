using LUIECompiler.Common;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration
{
    /// <summary>
    /// Represents the context for code generation.
    /// </summary>
    public class CodeGenerationContext
    {
        /// <summary>
        /// The constants in the program.
        /// </summary>
        public List<Constant<int>> IntegerConstants { get; } = [];

        /// <summary>
        /// The current code block being generated.
        /// </summary>
        public required CodeBlock CurrentBlock { get; set; }

        /// <summary>
        /// The symbol table for the program.
        /// </summary>
        public required SymbolTable SymbolTable { get; init; }

        public CodeGenerationContext()
        {
            
        }

        /// <summary>
        /// Creates a new code generation context with the given integer constants.
        /// </summary>
        /// <param name="integerConstants"></param>
        public CodeGenerationContext(List<Constant<int>> integerConstants)
        {
            IntegerConstants = integerConstants;
        }
    }
}