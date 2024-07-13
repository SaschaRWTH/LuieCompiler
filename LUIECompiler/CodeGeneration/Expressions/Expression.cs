using System.Numerics;
using LUIECompiler.Common;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Expressions
{
    public abstract class Expression<T> where T : INumber<T>
    {
        /// <summary>
        /// Evaluates the expression and returns the result.
        /// </summary>
        /// <param name="constants"></param>
        /// <returns></returns>
        public abstract T Evaluate(List<Constant<T>> constants);

        /// <summary>
        /// Gets the list of undefined identifiers in the expression.
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public abstract List<string> UndefinedIdentifiers(SymbolTable table);
    }
}