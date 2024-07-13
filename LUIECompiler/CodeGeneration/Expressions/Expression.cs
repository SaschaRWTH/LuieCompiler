using LUIECompiler.Common;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Expressions
{
    public abstract class Expression<T> 
    {
        public abstract T Evaluate(List<Constant<T>> constants);

        public abstract List<string> UndefinedIdentifiers(SymbolTable table);
    }
}