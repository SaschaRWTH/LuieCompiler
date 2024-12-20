using LUIECompiler.Common.Errors;
using LUIECompiler.SemanticAnalysis;

namespace LUIECompilerTests.SemanticAnalysis;

[TestClass, TestCategory("SemanticAnalysis"), TestCategory("ForLoop")]
public class ForLoopTest
{            
    public const string InputCorrect =
        "qubit c;\n" +
        "qubit a;\n" +
        "for i in 1..3 do\n" +
        "    h c;\n" +
        "    cx c, a;\n" +
        "end";

    public const string InputRedefine =
        "qubit c;\n" +
        "qubit a;\n" +
        "for a in 1..3 do\n" + // Use of already defined identifier
        "    h c;\n" +
        "    cx c, a;\n" +
        "end";

    public const string InputIteratorExpressionCorrect = 
        "qubit[3] c;\n" +
        "for i in 1..3 do\n" +
        "    h c[i];\n" +
        "end";

    public const string InputIteratorExpressionIncorrect = 
        "qubit[3] c;\n" +
        "qubit b;\n" +
        "for i in 1..3 do\n" +
        "    h c[b];\n" +
        "end";

    public const string InvalidRangeInput = 
        "qubit[3] c;\n" +
        "for i in 3..1 do\n" +
        "    h c[i];\n" +
        "end";

    public const string InvalidRedefinitionInput = 
        "qubit[5] i;\n" +
        "qubit[5] c;\n" +
        "for i in 0..4 do\n" +
        "    h c[i];\n" +
        "end\n"+
        "for j in 0..4 do\n" +
        "    h c[j];\n" +
        "    for j in 0..3 do\n" +
        "        h c[j];\n" +
        "    end\n" +
        "end";

    
    public const string DoubledLoop = @"
        qubit[2] a;
        for i in 0..1 do
            h a[i];
        end
        for i in 0..1 do
            h a[i];
        end
    ";

    /// <summary>
    /// Test no error are incorrectly reported.
    /// </summary>
    [TestMethod]
    public void CorrectInputTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(InputCorrect);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(!error.ContainsCriticalError);
    }

    /// <summary>
    /// Test that already defined identifiers are correctly reported.
    /// </summary>
    [TestMethod]
    public void RedefineErrorTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(InputRedefine);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(error.ContainsCriticalError);
        Assert.IsTrue(error.Errors.Any(e => e is RedefineError && e.ErrorContext.Line == 3));
    }

    /// <summary>
    /// Test no error are incorrectly reported.
    /// </summary>
    [TestMethod]
    public void CorrectIteratorUseTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(InputIteratorExpressionCorrect);
        var analysis = new TypeCheckListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(!error.ContainsCriticalError);
    }

    /// <summary>
    /// Test no error are incorrectly reported.
    /// </summary>
    [TestMethod]
    public void IncorrectIteratorUseTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(InputIteratorExpressionIncorrect);
        var analysis = new TypeCheckListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(error.ContainsCriticalError);
        Assert.IsTrue(error.Errors.Any(e => e is TypeError && e.ErrorContext.Line == 4));
    }

    /// <summary>
    /// Test no error are incorrectly reported.
    /// </summary>
    [TestMethod]
    public void InvalidRangeTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(InvalidRangeInput);
        var analysis = new TypeCheckListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(!error.ContainsCriticalError);
        Assert.IsTrue(error.Warnings.Count != 0);
        Assert.IsTrue(error.Errors.Any(e => e is InvalidRangeWarning && e.ErrorContext.Line == 2));
    }

    /// <summary>
    /// Test no error are incorrectly reported.
    /// </summary>
    [TestMethod]
    public void InvalidRedefinitionTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(InvalidRedefinitionInput);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Assert.IsTrue(error.ContainsCriticalError);
        Assert.IsTrue(error.Warnings.Count == 0);
        Assert.IsTrue(error.Errors.Any(e => e is RedefineError && e.ErrorContext.Line == 3));
        Assert.IsTrue(error.Errors.Any(e => e is RedefineError && e.ErrorContext.Line == 8));
    }

    /// <summary>
    /// Test no error are incorrectly reported.
    /// </summary>
    [TestMethod]
    public void DoubledLoopTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(DoubledLoop);
        var analysis = new DeclarationAnalysisListener();
        walker.Walk(analysis, parser.parse());
        var error = analysis.Error;

        Console.WriteLine(error);
        Assert.IsFalse(error.ContainsCriticalError);
        Assert.IsTrue(error.Warnings.Count == 0);
    }

}