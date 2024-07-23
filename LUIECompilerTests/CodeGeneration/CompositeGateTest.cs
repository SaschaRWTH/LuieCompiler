using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common.Errors;

namespace LUIECompilerTests.CodeGeneration;

[TestClass, TestCategory("CodeGeneration"), TestCategory("CompositeGate")]
public class CompositeGateTest
{
    public const string SwapDefinition =
        "gate swap(a, b) do\n" +
        "    cx a, b;\n" +
        "    cx b, a;\n" +
        "    cx a, b;\n" +
        "end\n" +
        "\n" +
        "qubit b;\n" +
        "qubit c;\n" +
        "x c;\n" +
        "swap b, c;";

    public const string SwapDefinitionTranslation =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "x id1;\n" +
        "cx id0, id1;\n" +
        "cx id1, id0;\n" +
        "cx id0, id1;\n";

    public const string DefinitionsUsingCompositeGate =
        "gate swap(a, b) do\n" +
        "    cx a, b;\n" +
        "    cx b, a;\n" +
        "    cx a, b;\n" +
        "end\n" +
        "gate paws(b, a) do\n" +
        "    swap a, b;\n" +
        "end\n" +
        "\n" +
        "qubit b;\n" +
        "qubit c;\n" +
        "x c;\n" +
        "paws b, c;";
        
    public const string DefinitionsUsingCompositeGateTranslation =
        "qubit id0;\n" +
        "qubit id1;\n" +
        "x id1;\n" +
        "cx id1, id0;\n" +
        "cx id0, id1;\n" +
        "cx id1, id0;\n";

        
    public const string InvalidRecursiveGate =
        "gate swap(a, b) do\n" +
        "    cx a, b;\n" +
        "    cx b, a;\n" +
        "    cx a, b;\n" +
        "end\n" +
        "gate paws(b, a) do\n" +
        "    paws a, b;\n" +
        "end\n" +
        "\n" +
        "qubit b;\n" +
        "qubit c;\n" +
        "x c;\n" +
        "paws b, c;";

    /// <summary>
    /// Tests the code generation for the simple input.
    /// </summary>
    [TestMethod]
    public void SimpleInputTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(SwapDefinition);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(SwapDefinitionTranslation, code);
    }

    /// <summary>
    /// Tests the code generation for a composite gate using another composite gate in its definition.
    /// </summary>
    [TestMethod]
    public void DefinitionsUsingCompositeGateTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(DefinitionsUsingCompositeGate);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(DefinitionsUsingCompositeGateTranslation, code);
    }

    /// <summary>
    /// Tests the the code generation throws the correct error for a recursive gate definition.
    /// </summary>
    [TestMethod]
    public void InvalidRecursiveGateTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(InvalidRecursiveGate);

        var codegen = new CodeGenerationListener();
        
        var exception = Assert.ThrowsException<CodeGenerationException>(() => walker.Walk(codegen, parser.parse()));

        Assert.IsTrue(exception.Error is UndefinedError error && error.Identifier[0] == "paws");
    }
}