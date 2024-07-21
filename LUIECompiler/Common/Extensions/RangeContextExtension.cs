using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.CodeGeneration.Expressions;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.Common.Extensions
{
    public static class RangeContextExtension
    {
        /// <summary>
        /// Get the range from the range context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="InternalException"></exception>
        public static LoopIterator GetRange(this LuieParser.RangeContext context, string identifier)
        {
            if (int.TryParse(context.start?.Text, out int start) && int.TryParse(context.end?.Text, out int end))
            {
                ConstantExpression<int> startExpression = new()
                {
                    Value = start,
                };
                ConstantExpression<int> endExpression = new()
                {
                    Value = end,
                };

                return new(identifier, startExpression, endExpression, new(context));
            }

            var length = context.length;
            if(length != null)
            {
                ConstantExpression<int> startExpression = new()
                {
                    Value = 0,
                };
                ConstantExpression<int> oneExpression = new()
                {
                    Value = 1,
                };
                BinaryOperationExpression<int> endExpression = new()
                {
                    Left = length.GetExpression<int>(),
                    Right = oneExpression,
                    Operator = BinaryOperator<int>.FromString("-"),
                };
                return new(identifier, startExpression, endExpression, new(context));
            }

            throw new InternalException()
            {
                Reason = "Failed to parse the range of the for statement.",
            };
        }
    }
}