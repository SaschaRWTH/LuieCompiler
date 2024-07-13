using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Expressions
{
    public class IdentifierExpression<T> : Expression<T>
    {
        public required string Identifier { get; init; }

        public override List<string> UndefinedIdentifiers(SymbolTable table)
        {
            List<string> undefined = new();
            if (!table.IsDefined(Identifier))
            {
                undefined.Add(Identifier);
            }

            return undefined;
        }

        public override T Evaluate(List<Constant<T>> constants)
        {
            // Find the constant with the given identifier
            var constant = constants.Find(constant => constant.Identifier == Identifier);
            if (constant == null)
            {
                throw new InternalException()
                {
                    Reason = $"Constant with identifier {Identifier} not found.",
                };
            }
            return constant.Value;
        }
    }
}