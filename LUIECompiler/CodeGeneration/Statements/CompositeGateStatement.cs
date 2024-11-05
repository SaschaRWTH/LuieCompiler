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
        public required Dictionary<GateArgument, Symbol> Arguments { get; init; }

        /// <summary>
        /// Returns the QASM code for the statement.
        /// </summary>
        /// <returns></returns>
        public override QASMProgram ToQASM(CodeGenerationContext context)
        {
            Dictionary<GateArgument, Symbol> argMap = [];
            foreach (var arg in context.ArgumentMap)
            {
                // The expressions need to be evaluated because some symbols may not be propagated (e.g. iterators).
                Symbol symbol = arg.Value;
                if (symbol is GateArgumentAccess access)
                {
                    symbol = new GateArgumentAccess(access.Argument, new ConstantExpression<int>(){
                        Value = access.IndexExpression.Evaluate(context),
                    }, access.ErrorContext);
                }
                argMap[arg.Key] = symbol;
            }
            foreach (var arg in Arguments)
            {
                Symbol symbol = arg.Value;
                if (symbol is GateArgumentAccess access)
                {
                    symbol = new GateArgumentAccess(access.Argument, new ConstantExpression<int>(){
                        Value = access.IndexExpression.Evaluate(context),
                    }, access.ErrorContext);
                }
                argMap[arg.Key] = symbol;
            }

            CodeGenerationContext bodyContext = new CodeGenerationContext(argMap)
            {
                CurrentBlock = context.CurrentBlock,
                // Propagate an empty symbol table, otherwise can access symbols NOT in scope of gate declaration.
                SymbolTable = new(),
            };

            return Gate.Body.ToQASM(bodyContext);
        }
    }
}