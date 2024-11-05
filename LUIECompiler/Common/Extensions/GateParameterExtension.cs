using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.Common.Extensions
{
    public static class GateParameterExtension
    {
        /// <summary>
        /// Gets the arguments used in the gate from the <paramref name="context"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<GateArgument> GetArguments(this LuieParser.GateParameterContext context)
        {
            List<GateArgument> args = [];

            foreach (string identifier in context.IDENTIFIER().Select(x => x.GetText()))
            {
                args.Add(new GateArgument(identifier, new ErrorContext(context)));
            }

            return args;
        }
    }
}