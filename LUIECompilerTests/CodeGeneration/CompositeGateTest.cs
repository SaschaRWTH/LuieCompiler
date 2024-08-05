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

    public const string SimpleForLoopGate =   
        "gate hTwice(reg) do\n" +
        "    for i in 0..1 do\n" +
        "        h reg;\n" +
        "    end\n" +
        "end\n" +
        "qubit b;\n" +
        "hTwice b;";
        
    public const string SimpleForLoopGateTranslation = 
        "qubit id0;\n" +
        "h id0;\n" +
        "h id0;\n";

    public const string ApplyGateToRegister = 
        "gate h_reg(reg) do\n" +
        "    for i in range(sizeof(reg)) do\n" +
        "        h reg[i];\n" +
        "    end\n" +
        "end\n" +
        "qubit[10] b;\n" +
        "h_reg b;";

    public const string ApplyGateToRegisterTranslation = 
        "qubit[10] id0;\n" +
        "h id0[0];\n" +
        "h id0[1];\n" +
        "h id0[2];\n" +
        "h id0[3];\n" +
        "h id0[4];\n" +
        "h id0[5];\n" +
        "h id0[6];\n" +
        "h id0[7];\n" +
        "h id0[8];\n" +
        "h id0[9];\n";

    public const string BasicIfGate = 
        "gate reimpl_cx(control, qbit) do\n" +
        "    qif control do\n" +
        "        x qbit;\n" +
        "    end\n" +
        "end\n" +
        "qubit a;\n" +
        "qubit b;\n" +
        "x a;\n" +
        "reimpl_cx a, b;";
    public const string BasicIfGateTranslation = 
        "qubit id0;\n" +
        "qubit id1;\n" +
        "x id0;\n" +
        "ctrl(1) @ x id0, id1;\n";

    public const string IfForCombinationGate = 
        "gate cx_reg(control, reg) do\n" +
        "    qif control do\n" +
        "        for i in range(sizeof(reg)) do\n" +
        "            x reg[i];\n" +
        "        end\n" +
        "    end\n" +
        "end\n" +
        "qubit a;\n" +
        "qubit[10] b;\n" +
        "x a;\n" +
        "cx_reg a, b;";

    public const string IfForCombinationGateTranslation =
        "qubit id0;\n" +
        "qubit[10] id1;\n" +
        "x id0;\n" +
        "ctrl(1) @ x id0, id1[0];\n" +
        "ctrl(1) @ x id0, id1[1];\n" +
        "ctrl(1) @ x id0, id1[2];\n" +
        "ctrl(1) @ x id0, id1[3];\n" +
        "ctrl(1) @ x id0, id1[4];\n" +
        "ctrl(1) @ x id0, id1[5];\n" +
        "ctrl(1) @ x id0, id1[6];\n" +
        "ctrl(1) @ x id0, id1[7];\n" +
        "ctrl(1) @ x id0, id1[8];\n" +
        "ctrl(1) @ x id0, id1[9];\n";

    public const string IfRegisterAccessGate = 
        "gate c_regx(control, reg) do\n" +
        "    for i in range(sizeof(control)) do\n" +
        "       qif control[i] do\n" +
        "            x reg;\n" +
        "        end\n" +
        "    end\n" +
        "end\n" +
        "qubit a;\n" +
        "qubit[10] b;\n" +
        "x a;\n" +
        "c_regx b, a;";

    public const string IfRegisterAccessGateTranslation =
        "qubit id0;\n" +
        "qubit[10] id1;\n" +
        "x id0;\n" +
        "ctrl(1) @ x id1[0], id0;\n" +
        "ctrl(1) @ x id1[1], id0;\n" +
        "ctrl(1) @ x id1[2], id0;\n" +
        "ctrl(1) @ x id1[3], id0;\n" +
        "ctrl(1) @ x id1[4], id0;\n" +
        "ctrl(1) @ x id1[5], id0;\n" +
        "ctrl(1) @ x id1[6], id0;\n" +
        "ctrl(1) @ x id1[7], id0;\n" +
        "ctrl(1) @ x id1[8], id0;\n" +
        "ctrl(1) @ x id1[9], id0;\n";

    public const string RegisterAccessArgForDefinedGateinDefinedGate =
    @"
        gate hnew(a) do
            h a;
        end

        gate first_h(a) do
            hnew a[0];
        end
        qubit[2] a;
        first_h a;
    ";

    public const string RegisterAccessArgForDefinedGateinDefinedGateTranslation =
        "qubit[2] id0;\n" +
        "h id0[0];\n";

    public const string DeeplyNestedCalls =
    @"
        gate hnew(a) do
            h a;
        end

        gate fourth_h(a) do
            for i in range(sizeof(a)) do
                hnew a[i];
            end
        end

        gate third_h(a) do
            fourth_h a;
        end

        gate second_h(a) do
            third_h a;
        end

        gate first_h(a) do
            second_h a;
        end
        qubit[2] a;
        first_h a;
    ";
    
    public const string DeeplyNestedCallsTranslation =
        "qubit[2] id0;\n" +
        "h id0[0];\n" +
        "h id0[1];\n";

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

    /// <summary>
    /// Tests the code generation for a simple for loop inside a gate definition.
    /// </summary>
    [TestMethod]
    public void SimpleForLoopGateTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(SimpleForLoopGate);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(SimpleForLoopGateTranslation, code);
    }

    /// <summary>
    /// Tests the code generation for a for loop based on the size of a given register.
    /// </summary>
    [TestMethod]
    public void ApplyGateToRegisterTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(ApplyGateToRegister);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(ApplyGateToRegisterTranslation, code);
    }

    /// <summary>
    /// Tests the code generation for an if statement inside a gate definition.
    /// </summary>
    [TestMethod]
    public void BasicIfGateTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(BasicIfGate);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(BasicIfGateTranslation, code);
    }

    /// <summary>
    /// Tests the code generation for a combination of if and for loop inside a gate definition.
    /// </summary>
    [TestMethod]
    public void IfForCombinationGateTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(IfForCombinationGate);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(IfForCombinationGateTranslation, code);
    }

    /// <summary>
    /// Tests the code generation for a combination of if and for loop inside a gate definition.
    /// </summary>
    [TestMethod]
    public void IfRegisterAccessGateTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(IfRegisterAccessGate);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(IfRegisterAccessGateTranslation, code);
    }

    /// <summary>
    /// Tests the code generation for a register access as an argument for a defined gate inside a defined gate.
    /// </summary>
    [TestMethod]
    public void RegisterAccessArgForDefinedGateinDefinedGateTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(RegisterAccessArgForDefinedGateinDefinedGate);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(RegisterAccessArgForDefinedGateinDefinedGateTranslation, code);
    }

    /// <summary>
    /// Tests the code generation deeply nested calls of defined gates.
    /// </summary>
    [TestMethod]
    public void DeeplyNestedCallsTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(DeeplyNestedCalls);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        string? code = codegen.CodeGen.GenerateCode()?.ToString();
        Assert.IsNotNull(code);

        Assert.AreEqual(DeeplyNestedCallsTranslation, code);
    }
}