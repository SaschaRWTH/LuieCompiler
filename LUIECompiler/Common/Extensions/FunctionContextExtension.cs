using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.CodeGeneration.Expressions;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.Common.Extensions
{
    public static class FunctionContextExtension
    {
        /// <summary>
        /// Gets the function type of the <see cref="LuieParser.FunctionContext"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static FunctionType GetFunctionType(this LuieParser.FunctionContext context)
        {
            return context.func.Text switch
            {
                "sizeof" => FunctionType.SizeOf,
                _ => throw new NotImplementedException()
            };
        }

        /// <summary>
        /// Gets the parameters of the function.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        /// <exception cref="CodeGenerationException"></exception>
        public static List<string> GetFunctionParameters(this LuieParser.FunctionContext context)
        {
            string identifier = context.param.Text;

            return [identifier];
        }
    }
}