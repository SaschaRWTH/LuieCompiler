
using LUIECompiler.CodeGeneration.Definitions;
using LUIECompiler.CodeGeneration.Statements;
using LUIECompiler.Common;

namespace LUIECompiler.CodeGeneration
{
    public class CodeGenerationHandler 
    {
        public SymbolTable Table { get; set; } = new();
        public List<AbstractDefinition> Definitions { get; } = [];
        public List<AbstractStatement> Block { get; } = [];
    }

}