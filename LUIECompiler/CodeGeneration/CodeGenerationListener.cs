using System.Data.Common;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace LUIECompiler.CodeGeneration
{
    public class CodeGenerationListener : LuieBaseListener
    {
        public override void EnterBlock([NotNull] LuieParser.BlockContext context)
        {
            
            base.EnterBlock(context);
        }

        public override void ExitBlock([NotNull] LuieParser.BlockContext context)
        {
            base.ExitBlock(context);
        }
    }

}
