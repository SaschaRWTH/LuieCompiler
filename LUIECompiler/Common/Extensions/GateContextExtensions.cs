using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.Common.Extensions
{
    public static class GateContextExtension
    {
        public static IGate GetGate(this LuieParser.GateContext context, SymbolTable symbolTable)
        {
            if (context.type?.Text is string type)
            {
                return FromString(type);
            }

            string identifier = context.identifier.Text;
            Symbol? symbol = symbolTable.GetSymbolInfo(identifier);

            if (symbol is not CompositeGate compositeGate)
            {
                throw new CodeGenerationException()
                {
                    Error = new UndefinedError(new ErrorContext(context.Start), identifier),
                };
            }

            return compositeGate;
        }

        private static Gate FromString(string gate)
        {
            GateType type = GateTypeExtensions.FromString(gate);

            return new()
            {
                NumberOfArguments = type.GetNumberOfArguments(),
                Type = type,
            };
        }
    }
}