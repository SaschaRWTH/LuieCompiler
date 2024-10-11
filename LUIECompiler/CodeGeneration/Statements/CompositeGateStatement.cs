using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.CodeGeneration.Expressions;
using LUIECompiler.Common.Symbols;

namespace LUIECompiler.CodeGeneration.Statements
{
    /// <summary>
    /// Represents a composite gate statement.
    /// </summary>
    public class CompositeGateStatement : Statement
    {
        /// <summary>
        /// Gate that is applied in the statement.
        /// </summary>
        public required CompositeGate Gate { get; init; }

        /// <summary>
        /// Register to which the gate is applied to.
        /// </summary>
        public required Dictionary<Parameter, Symbol> Parameters { get; init; }

        /// <summary>
        /// Returns the QASM code for the statement.
        /// </summary>
        /// <returns></returns>
        public override QASMProgram ToQASM(CodeGenerationContext context)
        {
            Dictionary<Parameter, Symbol> parameterMap = [];
            foreach (var parameter in context.ParameterMap)
            {
                // The expressions need to be evaluated because some symbols may not be propagated (e.g. iterators).
                Symbol symbol = parameter.Value;
                if (symbol is ParameterAccess access)
                {
                    symbol = new ParameterAccess(access.Parameter, new ConstantExpression<int>(){
                        Value = access.IndexExpression.Evaluate(context),
                    }, access.ErrorContext);
                }
                parameterMap[parameter.Key] = symbol;
            }
            foreach (var parameter in Parameters)
            {
                Symbol symbol = parameter.Value;
                if (symbol is ParameterAccess access)
                {
                    symbol = new ParameterAccess(access.Parameter, new ConstantExpression<int>(){
                        Value = access.IndexExpression.Evaluate(context),
                    }, access.ErrorContext);
                }
                parameterMap[parameter.Key] = symbol;
            }

            CodeGenerationContext bodyContext = new CodeGenerationContext(parameterMap)
            {
                CurrentBlock = context.CurrentBlock,
                // Propagate an empty symbol table, otherwise can access symbols NOT in scope of gate declaration.
                SymbolTable = new(),
            };

            return Gate.Body.ToQASM(bodyContext);
        }
    }
}