using Antlr4.Runtime;

namespace LUIECompiler.Common.Errors
{
    /// <summary>
    /// Represents the context of an error.
    /// </summary>
    public struct ErrorContext
    {
        /// <summary>
        /// Line where the error occured.
        /// </summary>
        public int Line { get; set; }

        /// <summary>
        /// Column where the error occured.
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// Creates a new error context.
        /// </summary>
        /// <param name="line">Line where the error occured.</param>
        /// <param name="column">Column where the error occured.</param>
        public ErrorContext(int line, int column)
        {
            Line = line;
            Column = column;
        }

        /// <summary>
        /// Creates a new error context from a <paramref name="token"/>.
        /// </summary>
        /// <param name="token"></param>
        public ErrorContext(IToken token)
        {
            Line = token.Line;
            Column = token.Column;
        }

        /// <summary>
        /// Create a new error context from the <paramref name="context"/>.
        /// </summary>
        /// <param name="context"></param>
        public ErrorContext(ParserRuleContext context)
        {
            Line = context.Start.Line;
            Column = context.Start.Column;
        }
    }
}