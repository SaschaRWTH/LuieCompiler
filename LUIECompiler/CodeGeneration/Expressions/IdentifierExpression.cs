using System.Numerics;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Expressions
{
    /// <summary>
    /// Represents an identifier expression which evaluates to the value of the identifier.
    /// </summary>
    /// <typeparam name="T">Type of the identifier value.</typeparam>
    public class IdentifierExpression<T> : Expression<T> where T : INumber<T>
    {
        /// <summary>
        /// Identifier of the constant.
        /// </summary>
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
            Symbol symbol = context.SymbolTable.GetSymbolInfo(Identifier) ?? throw new InternalException()
            {
                Reason = $"Symbol with identifier {Identifier} not found.",
            };

            if (symbol is Constant<T> constant)
            {
                return constant.Value;
            }
            else if(symbol is LoopIterator loopIterator)
            {
                return T.CreateChecked(loopIterator.CurrentValue);
            }
            else
            {
                throw new InternalException()
                {
                    Reason = $"Symbol with identifier {Identifier} is not a constant.",
                };
            }

            // Find the constant with the given identifier
            // var constant = context.IntegerConstants.Find(constant => constant.Identifier == Identifier);
            // if (constant == null)
            // {
            //     throw new InternalException()
            //     {
            //         Reason = $"Constant with identifier {Identifier} not found.",
            //     };
            // }
            // return T.CreateChecked(constant.Value);
        }

        public override string ToString()
        {
            return Identifier;
        }
    }
}