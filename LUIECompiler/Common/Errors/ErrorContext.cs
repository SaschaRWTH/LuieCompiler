using Antlr4.Runtime;

namespace LUIECompiler.Common.Errors{
    public struct ErrorContext
    {
        public int Line { get; set; }
        
        public int Column { get; set; }

        public ErrorContext(int line, int column)
        {
            Line = line;
            Column = column;
        }

        public ErrorContext(IToken token)
        {
            Line = token.Line;
            Column = token.Column;
        }
        public ErrorContext(ParserRuleContext context)
        {
            Line = context.Start.Line;
            Column = context.Start.Column;
        }
    }
}