using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using LUIECompiler.Common;
using LUIECompiler.Common.Errors;
using LUIECompiler.Common.Symbols;
using LUIECompiler.SemanticAnalysis;

namespace LUIECompilerTests;

[TestClass]
public class SemanticAnalysisTest
{
    public const string InputSimple =
        "qubit c;\n" +
        "qubit b;\n" +
        "x y;\n" +
        "qubit c;\n" +
        "qif y do\n" +
        "skip;\n" +
        "end";

    public const string InputScopeCorrect =
        "qubit a;\n" +
        "qif a do\n" +
        "qubit a;\n" +
        "qif a do\n" +
        "qubit a;\n" +
        "end\n" +
        "end";

    public const string InputScopeIncorrect =
        "qubit a;\n" +
        "qif a do\n" +
        "qubit a;\n" +
        "qif a do\n" +
        "qubit a;\n" +
        "qubit a;\n" +
        "end\n" +
        "end";


    public const string InputScopeUseOfOuterScope =
        "qubit a;\n" +
        "qif a do\n" +
        "qubit b;\n" +
        "qif a do\n" +
        "qubit a;\n" +
        "end\n" +
        "end";

    /// <summary>
    /// Gets a walker.
    /// </summary>
    /// <returns></returns>
    public ParseTreeWalker GetWalker()
    {
        ParseTreeWalker walker = new();
        return walker;
    }

    /// <summary>
    /// Creates a parser for the <paramref name="input"/>.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public LuieParser GetParser(string input)
    {
        AntlrInputStream inputStream = new AntlrInputStream(input.ToString());
        LuieLexer luieLexer = new LuieLexer(inputStream);
        CommonTokenStream commonTokenStream = new CommonTokenStream(luieLexer);
        LuieParser luieParser = new LuieParser(commonTokenStream);
        return luieParser;
    }

    /// <summary>
    /// Test that already defined identifiers are correctly reported.
    /// </summary>
    [TestMethod]
    public void RedefineErrorTest()
    {
        var walker = GetWalker();
        var parser = GetParser(InputSimple);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(error.ContainsCriticalError);

        Assert.IsTrue(error.Errors.Any(e => e is RedefineError && e.Line == 4));
    }

    /// <summary>
    /// Tests that undefined identifiers are correctly reported.
    /// </summary>
    [TestMethod]
    public void UndefinedErrorTest()
    {
        var walker = GetWalker();
        var parser = GetParser(InputSimple);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(error.ContainsCriticalError);

        Assert.IsTrue(error.Errors.Any(e => e is UndefinedError && e.Line == 3));
        Assert.IsTrue(error.Errors.Any(e => e is UndefinedError && e.Line == 5));
    }

    /// <summary>
    /// Test that in a simple, correct program with scopes there are no errors reported. 
    /// </summary>
    [TestMethod]
    public void ScopeCorrectTest()
    {
        var walker = GetWalker();
        var parser = GetParser(InputScopeCorrect);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(!error.ContainsCriticalError);
    }

    /// <summary>
    /// Test that redefinitions in correctly reported in scopes.
    /// </summary>
    [TestMethod]
    public void ScopeIncorrectTest()
    {
        var walker = GetWalker();
        var parser = GetParser(InputScopeIncorrect);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(error.ContainsCriticalError);
        Assert.IsTrue(error.Errors.Any(e => e is RedefineError && e.Line == 6));
    }

    /// <summary>
    /// Tests that there are no errors reported when using an identifier defined in an outer scope.
    /// </summary>
    [TestMethod]
    public void ScopeUseOfOuterScopeTest()
    {
        var walker = GetWalker();
        var parser = GetParser(InputScopeUseOfOuterScope);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(!error.ContainsCriticalError);
    }

    /// <summary>
    /// Tests whether the correct symbol is returned when using the same identifier in different scopes.
    /// </summary>
    [TestMethod]
    public void CorrectInfoTest()
    {
        SymbolTable table = new();
        
        table.PushScope();
        Register firstA = new("A");
        table.AddSymbol(firstA);

        table.PushScope();
        Register secondA = new("A");
        table.AddSymbol(secondA);

        Register? secondScopeA = table.GetSymbolInfo("A") as Register;
        Assert.IsNotNull(secondScopeA);
        Assert.AreNotEqual(firstA, secondScopeA);
        Assert.AreEqual(secondA, secondScopeA);

        table.PopScope();
        Register? firstScopeA = table.GetSymbolInfo("A") as Register;
        Assert.IsNotNull(secondScopeA);
        Assert.AreNotEqual(secondA, firstScopeA);
        Assert.AreEqual(firstA, firstScopeA);
    }
}