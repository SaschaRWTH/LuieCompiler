using System.Numerics;
using LUIECompiler.Common;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Expressions
{
    public class ConstantExpression<T> : Expression<T> where T : INumber<T>
    {
        public required T Value { get; init; }

        public override T Evaluate(List<Constant<T>> constants) 
        { 
            return Value;
        }

        public override List<string> UndefinedIdentifiers(SymbolTable table)
        {
            return new();
        }
    }
}