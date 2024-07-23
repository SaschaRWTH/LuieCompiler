using System.Numerics;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Expressions
{
    public class IdentifierExpression<T> : Expression<T> where T : INumber<T>
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

        public override T Evaluate(CodeGenerationContext context)
        {
            // Find the constant with the given identifier
            var constant = context.IntegerConstants.Find(constant => constant.Identifier == Identifier);
            if (constant == null)
            {
                throw new InternalException()
                {
                    Reason = $"Constant with identifier {Identifier} not found.",
                };
            }
            return T.CreateChecked(constant.Value);
        }
    }
}