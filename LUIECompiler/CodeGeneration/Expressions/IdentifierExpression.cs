using System.Numerics;
using System.Runtime.CompilerServices;
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

        public Symbol? Symbol { get; set; }

        public override List<string> PropagateSymbolInformation(SymbolTable table)
        {
            List<string> undefined = new();

            Symbol = table.GetSymbolInfo(Identifier);

            if (Symbol is null)
            {
                undefined.Add(Identifier);
            }

            return undefined;
        }

        public override T Evaluate(CodeGenerationContext context)
        {
            Symbol symbol = Symbol ?? throw new InternalException()
            {
                Reason = $"Symbol with identifier {Identifier} not found.",
            };
            // Symbol symbol = context.SymbolTable.GetSymbolInfo(Identifier) ?? throw new InternalException()
            // {
            //     Reason = $"Symbol with identifier {Identifier} not found.",
            // };
            
            if (symbol is Constant<T> constant)
            {
                return constant.Value.Evaluate(context);
            }
            else if (symbol is Constant<int> intConstant)
            {
                return T.CreateChecked(intConstant.Value.Evaluate(context));
            }
            else if (symbol is Constant<double> doubleConstant)
            {
                return T.CreateChecked(doubleConstant.Value.Evaluate(context));
            }
            else if (symbol is Constant<uint> uintConstant)
            {
                return T.CreateChecked(uintConstant.Value.Evaluate(context));
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
        }

        public override string ToString()
        {
            return Identifier;
        }
    }
}