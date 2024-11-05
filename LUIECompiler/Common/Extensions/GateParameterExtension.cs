using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.Common.Extensions
{
    public static class GateParameterExtension
    {
        /// <summary>
        /// Gets the parameters used in the gate from the <paramref name="context"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<GateArgument> GetParameters(this LuieParser.GateParameterContext context)
        {
            List<GateArgument> parameters = [];

            foreach (string identifier in context.IDENTIFIER().Select(x => x.GetText()))
            {
                parameters.Add(new GateArgument(identifier, new ErrorContext(context)));
            }

            return parameters;
        }
    }
}