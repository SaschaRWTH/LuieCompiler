using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.CodeGeneration.Gates;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.Common.Extensions
{
    public static class GateContextExtension
    {
        public static Gate GetGate(this LuieParser.GateContext context, SymbolTable symbolTable)
        {
            if(context.type?.Text is string type)
            {
                return FromString(type);
            }

            string identifier = context.identifier.Text;
            Symbol? symbol = symbolTable.GetSymbolInfo(identifier);

            if(symbol is not CompositeGate compositeGate)
            {
                throw new CodeGenerationException()
                {
                    Error = new UndefinedError(new ErrorContext(context.Start), identifier),
                };
            }

            return new DefinedGate(compositeGate);
        }

        private static Gate FromString(string gate)
        {
            return gate switch
            {
                "x" => new XGate(),
                "y" => new YGate(),
                "z" => new ZGate(),
                "h" => new HGate(),
                "cx" => new ControlledXGate(),
                "ccx" => new ControlledControlledXGate(),
                _ => throw new NotImplementedException(),
            };
        }
    }
}