using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Errors;

namespace LUIECompilerTests.CodeGeneration;

[TestClass, TestCategory("CodeGeneration"), TestCategory("ForLoop")]
public class ForLoopTest
{            
    public const string InputCorrect =
        "qubit c;\n" +
        "qubit a;\n" +
        "for i in 1..3 do\n" +
        "    h c;\n" +
        "    cx c, a;\n" +
        "end";
     
    public const string InputCorrectTranslation =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "h id0;\n" +
        "cx id0, id1;\n" +
        "h id0;\n" +
        "cx id0, id1;\n" +
        "h id0;\n" +
        "cx id0, id1;\n";


    public const string InputRedefine =
        "qubit c;\n" +
        "qubit a;\n" +
        "for a in 1..3 do\n" + // Use of already defined identifier
        "    h c;\n" +
        "    cx c, a;\n" +
        "end";

    public const string InputIdenExpression = 
        "qubit[3] c;\n" +
        "qubit a;\n" +
        "for i in 0..2 do\n" +
        "    h c[i];\n" +
        "    cx c[i], a;\n" +
        "end";

    public const string InputIdenExpressionTranslation =
        "qubit[3] id0;\n" +
        "qubit id1;\n" +
        "h id0[0];\n" +
        "cx id0[0], id1;\n" +
        "h id0[1];\n" +
        "cx id0[1], id1;\n" +
        "h id0[2];\n" +
        "cx id0[2], id1;\n";

    public const string DefinitionInForLoop =
        "qubit[5] c;\n" +
        "for i in 0..4 do\n" +
        "    qubit a;\n" +
        "    h c[i];\n" +
        "    h a;\n" +
        "end\n" +
        "qubit a;\n" +
        "h a;";

    public const string DefinitionInForLoopTranslation =
        "qubit[5] id0;\n" +
        "qubit id1;\n" +
        "h id0[0];\n" +
        "h id1;\n" +
        "qubit id2;\n" +
        "h id0[1];\n" +
        "h id2;\n" +
        "qubit id3;\n" +
        "h id0[2];\n" +
        "h id3;\n" +
        "qubit id4;\n" +
        "h id0[3];\n" +
        "h id4;\n" +
        "qubit id5;\n" +
        "h id0[4];\n" +
        "h id5;\n" +
        "qubit id6;\n" +
        "h id6;\n";

    public const string InvalidIteratorInput =
        "qubit[5] c;\n" +
        "for i in 0..4 do\n" +
        "    qubit a;\n" +
        "    h c[i];\n" +
        "    h c[j];\n" +
        "    h a;\n" +
        "end\n";

    /// <summary>
    /// Test the translation of a simple for loop.
    /// </summary>
    [TestMethod]
    public void CorrectInputTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(InputCorrect);
        var gen = new CodeGenerationListener();
        walker.Walk(gen, parser.parse());
        
        string? code = gen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(code, InputCorrectTranslation);
    }

    /// <summary>
    /// Test that already defined identifiers are correctly reported.
    /// </summary>
    [TestMethod]
    public void RedefineErrorTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(InputRedefine);
        var gen = new CodeGenerationListener();
        CodeGenerationException e = Assert.ThrowsException<CodeGenerationException>(() 
            => walker.Walk(gen, parser.parse())
            );
        Assert.IsTrue(e.Error is RedefineError);
        Assert.AreEqual(3, e.Error.ErrorContext.Line);
    }

    /// <summary>
    /// Test the translation of a simple for loop.
    /// </summary>
    [TestMethod]
    public void InputIdenExpressionTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(InputIdenExpression);
        var gen = new CodeGenerationListener();
        walker.Walk(gen, parser.parse());
        
        string? code = gen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(InputIdenExpressionTranslation, code);
    }

    /// <summary>
    /// Test the translation of a simple for loop.
    /// </summary>
    [TestMethod]
    public void DefinitionInForLoopTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(DefinitionInForLoop);
        var gen = new CodeGenerationListener();
        walker.Walk(gen, parser.parse());
        
        string? code = gen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(DefinitionInForLoopTranslation, code);
    }

    /// <summary>
    /// Test the translation of a simple for loop.
    /// </summary>
    [TestMethod]
    public void InvalidIteratorInputTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(InvalidIteratorInput);
        var gen = new CodeGenerationListener();

        var ex = Assert.ThrowsException<CodeGenerationException>(() => walker.Walk(gen, parser.parse()));
        
        Assert.IsTrue(ex.Error is UndefinedError);
        Assert.AreEqual(5, ex.Error.ErrorContext.Line);
    }
}