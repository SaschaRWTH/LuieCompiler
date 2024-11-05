using LUIECompiler.Common.Symbols;

namespace LUIECompiler.Common.Extensions
{
    public static class GateDeclarationContextExtension
    {
        /// <summary>
        /// Gets the parameters used in the gate declaration from the <paramref name="context"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<GateArgument> GetArguments(this LuieParser.GateDeclarationContext context)
        {
            if(context.param is not LuieParser.GateParameterContext gateParameterContext)
            {
                return [];
            }
            return gateParameterContext.GetParameters();
        }
    }
}