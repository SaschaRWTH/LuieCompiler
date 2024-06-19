using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using LUIECompiler.Common.Errors;
using LUIECompiler.SemanticAnalysis;

namespace LUIECompilerTests;

[TestClass]
public class SemanticAnalysisTest
{
    public string Input =
        "qubit c;\n" +  
        "qubit b;\n" +
        "x y;\n" +
        "qubit c;\n" +
        "qif y do\n" +
        "skip;\n" +
        "end";

    public ParseTreeWalker GetWalker()
    {
        ParseTreeWalker walker = new();
        return walker;
    }

    public LuieParser GetParser(string input)
    {
        AntlrInputStream inputStream = new AntlrInputStream(input.ToString());
        LuieLexer luieLexer = new LuieLexer(inputStream);
        CommonTokenStream commonTokenStream = new CommonTokenStream(luieLexer);
        LuieParser luieParser = new LuieParser(commonTokenStream);
        return luieParser;
    }

    [TestMethod]
    public void RedefineErrorTest()
    {
        var walker = GetWalker();
        var parser = GetParser(Input);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(error.ContainsCriticalError);

        Assert.IsTrue(error.Errors.Any(e => e is RedefineError && e.Line == 4));
    }
    [TestMethod]
    public void UndefinedErrorTest()
    {
        var walker = GetWalker();
        var parser = GetParser(Input);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(error.ContainsCriticalError);

        Assert.IsTrue(error.Errors.Any(e => e is UndefinedError && e.Line == 3));
        Assert.IsTrue(error.Errors.Any(e => e is UndefinedError && e.Line == 5));
    }
}