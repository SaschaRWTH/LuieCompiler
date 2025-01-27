using System.Numerics;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Expressions
{
    /// <summary>
    /// Represents a sizeof function expression.
    /// </summary>
    /// <typeparam name="T">Type of the result.</typeparam>
    public class SizeOfFunctionExpression<T> : FunctionExpression<T> where T : INumber<T>
    {
        /// <summary>
        /// List of parameters of the function.
        /// </summary>
        private string Identifier { get; }

        public Symbol? Register { get; set; }

        /// <summary>
        /// Creates a sizeof function expression from the given <paramref name="context"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public SizeOfFunctionExpression(LuieParser.FunctionParameterContext context, SymbolTable symbolTable)
        {
            var argList = context.IDENTIFIER()?.Select(obj => obj.GetText())?.ToList() ?? throw new NotImplementedException();
            if (argList.Count != 1)
            {
                throw new CodeGenerationException()
                {
                    Error = new InvalidFunctionArguments(new ErrorContext(context), "SizeOf", 1, argList.Count),
                };
            }
            Identifier = argList[0];
            ArgumentErrorContext = new ErrorContext(context);


            // Ignore cases for undefined, will be checked by semantic analysis and get undefined identifiers
            Register = symbolTable.GetSymbolInfo(Identifier);
        }

        public override List<string> UndeclaredIdentifiers()
        {
            return Register is null ? [Identifier] : [];
        }

        public override T Evaluate(CodeGenerationContext context)
        {
            if (Register is null)
            {
                throw new InternalException()
                {
                    Reason = $"Symbol with identifier {Identifier} not found.",
                };
            }

            Symbol symbol = Register;
            if (Register is GateArgument argument)
            {
                symbol = argument.ToRegister(context);
            }

            if (symbol is not Register register)
            {
                Compiler.LogError($"SizeOf parameter '{Identifier}' is not a register.");
                throw new CodeGenerationException()
                {
                    Error = new TypeError(new ErrorContext(), Register.Identifier, typeof(Register), Register.GetType()),
                };
            }

            return T.CreateChecked(register.Size.Evaluate(context));
        }

        public override string ToString()
        {
            return $"sizeof({Identifier[0]})";
        }
    }
}