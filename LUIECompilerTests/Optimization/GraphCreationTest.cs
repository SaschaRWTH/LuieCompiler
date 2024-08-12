using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common;
using LUIECompiler.Optimization.Graphs;
using LUIECompiler.Optimization.Graphs.Interfaces;
using LUIECompiler.Optimization.Graphs.Nodes;

namespace LUIECompilerTests.Optimization;

[TestClass, TestCategory("Optimization")]
public class GraphCreationTest
{
    public const string SimpleInput =
    @"
        qubit c;
        qubit a;
        h c;
        z c;
        x a;
    ";


    [TestMethod]
    public void SimpleXXNullGateTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(SimpleInput);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        CircuitGraph graph = new(program);

        Assert.AreEqual(2, graph.Qubits.Count);

        var c = graph.Qubits[0];
        var a = graph.Qubits[1];

        Assert.AreEqual("id0", c.Identifier);
        Assert.AreEqual("id1", a.Identifier);

        var hc = CheckGateNode(c.Start, GateType.H);
        Assert.IsNotNull(hc);

        var xa = CheckGateNode(a.Start, GateType.X);
        Assert.IsNotNull(xa);

        var zhc = CheckGateNode(hc, GateType.Z);
        Assert.IsNotNull(zhc);
    }

    public GateNode? CheckGateNode(INode parent, GateType gateType)
    {
        Assert.AreEqual(1, parent.OutputVertices.Count);
        var cNode = parent.OutputVertices[0].End;
        Assert.IsTrue(cNode is GateNode);
        Assert.AreEqual(gateType, (cNode as GateNode)?.Gate);

        return cNode as GateNode;
    }
}
