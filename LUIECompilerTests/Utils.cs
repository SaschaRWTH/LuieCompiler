using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace LUIECompilerTests
{
    public static class Utils
    {
        /// <summary>
        /// Gets a walker.
        /// </summary>
        /// <returns></returns>
        public static ParseTreeWalker GetWalker()
        {
            ParseTreeWalker walker = new();
            return walker;
        }

        /// <summary>
        /// Creates a parser for the <paramref name="input"/>.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static LuieParser GetParser(string input)
        {
            AntlrInputStream inputStream = new AntlrInputStream(input.ToString());
            LuieLexer luieLexer = new LuieLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(luieLexer);
            LuieParser luieParser = new LuieParser(commonTokenStream);
            return luieParser;
        }
    }
}