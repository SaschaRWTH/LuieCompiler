using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.Common.Extensions
{
    public static class GateParameterExtension
    {
        public static List<Parameter> GetParameters(this LuieParser.GateParameterContext context)
        {
            List<Parameter> parameters = [];

            foreach (string identifier in context.IDENTIFIER().Select(x => x.GetText()))
            {
                parameters.Add(new Parameter(identifier, new ErrorContext(context)));
            }

            return parameters;
        }
    }
}