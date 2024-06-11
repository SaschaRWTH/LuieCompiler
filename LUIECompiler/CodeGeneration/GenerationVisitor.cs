using System.Data.Common;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace LUIECompiler.CodeGeneration
{
    public class GenerationVisitor : LuieBaseVisitor<CodeGenerationResult> 
    {
        public override CodeGenerationResult VisitBlock([NotNull] LuieParser.BlockContext context)
        {
            return base.VisitBlock(context);
        }

        public override CodeGenerationResult VisitDefinition([NotNull] LuieParser.DefinitionContext context)
        {
            foreach(var child in context.children)
            {
                
            }

            return base.VisitDefinition(context);
        }

        public override CodeGenerationResult VisitElseStat([NotNull] LuieParser.ElseStatContext context)
        {
            return base.VisitElseStat(context);
        }

        public override CodeGenerationResult VisitErrorNode(IErrorNode node)
        {
            return base.VisitErrorNode(node);
        }

        public override CodeGenerationResult VisitIfStat([NotNull] LuieParser.IfStatContext context)
        {
            return base.VisitIfStat(context);
        }

        public override CodeGenerationResult VisitParse([NotNull] LuieParser.ParseContext context)
        {
            return base.VisitParse(context);
        }

        public override CodeGenerationResult VisitQifStatement([NotNull] LuieParser.QifStatementContext context)
        {
            return base.VisitQifStatement(context);
        }

        public override CodeGenerationResult VisitStatement([NotNull] LuieParser.StatementContext context)
        {
            return base.VisitStatement(context);
        }

        public override CodeGenerationResult VisitTerminal(ITerminalNode node)
        {
            return base.VisitTerminal(node);
        }

        protected override CodeGenerationResult AggregateResult(CodeGenerationResult aggregate, CodeGenerationResult nextResult)
        {
            return aggregate.Append(nextResult);
        }
    }
    
}
