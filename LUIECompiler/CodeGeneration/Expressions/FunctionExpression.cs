using System.Numerics;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Expressions
{
    public abstract class FunctionExpression<T> : Expression<T> where T : INumber<T>
    {
        public static FunctionExpression<T> FromContext(LuieParser.FunctionContext context)
        {
            string function = context.func.Text;
            return function switch
            {
                "sizeof" => new SizeOfFunctionExpression<T>(context.param),
                "power" => new PowerFunctionExpression<T>(context.param),
                _ => throw new NotImplementedException(),
            };
        } 
    }
}