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

    public const string QFTInput =
    @"
        // Swaps the values of two qubits
        gate swap(a, b) do
            cx a, b;
            cx b, a;
            cx a, b;
        end

        // Performs a discrete Fourier transform on a register of qubits
        gate qft(reg) do
            for i in range(sizeof(reg)) do
                h reg[i];
                for j in range(sizeof(reg) - (i + 1)) do
                    qif reg[j + (i + 1)] do
                        p(1/(power(2, (j + 1)))) reg[i];
                    end
                end
            end
            for j in range(sizeof(reg) / 2) do
                swap reg[j], reg[sizeof(reg) - (j + 1)];
            end
        end


        qubit[5] a;
        qft a;
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

        Assert.AreEqual("id0", c.Identifier.Identifier);
        Assert.AreEqual("id1", a.Identifier.Identifier);

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

    /// <summary>
    /// Tests that for each qubit there exists exactly one path.
    /// </summary>
    [TestMethod]
    public void EachQubitSinglePathTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(QFTInput);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        CircuitGraph graph = new(program);

        foreach (var qubit in graph.Qubits)
        {
            CheckQubitPath(qubit);
        }
    }

    public void CheckQubitPath(GraphQubit qubit)
    {
        Assert.IsNotNull(qubit.Start);
        Assert.IsNotNull(qubit.End);

        INode current = qubit.Start;
        while(current != qubit.End)
        {
            var outV = current.OutputVertices.OfType<CircuitVertex>();
            Assert.IsTrue(outV.Count(v => v.Qubit == qubit) == 1);
            current = outV.Single(v => v.Qubit == qubit).End;
        }
    }

}
