using System.Numerics;
using LUIECompiler.Common;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Expressions
{
    /// <summary>
    /// Represents an abstract expression.
    /// </summary>
    /// <typeparam name="T">Type of the result of the expression</typeparam>
    public abstract class Expression<T> where T : INumber<T>
    {
        /// <summary>
        /// Evaluates the expression and returns the result.
        /// </summary>
        /// <param name="constants"></param>
        /// <returns></returns>
        public abstract T Evaluate(CodeGenerationContext context);

        /// <summary>
        /// Propagates the symbol information through an expression and return a list of undeclared identifiers.
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public abstract List<string> PropagateSymbolInformation(SymbolTable table);
    }
}