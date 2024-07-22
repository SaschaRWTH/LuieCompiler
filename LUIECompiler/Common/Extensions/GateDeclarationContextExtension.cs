using LUIECompiler.Common.Symbols;

namespace LUIECompiler.Common.Extensions
{
    public static class GateDeclarationContextExtension
    {
        public static List<Parameter> GetParameters(this LuieParser.GateDeclarationContext context)
        {
            if(context.param is not LuieParser.GateParameterContext gateParameterContext)
            {
                return [];
            }
            
            return gateParameterContext.GetParameters();
        }
    }
}