using LUIECompiler.CodeGeneration;
using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.Common;
using LUIECompiler.Optimization.Graphs;
using LUIECompiler.Optimization.Graphs.Interfaces;
using LUIECompiler.Optimization.Graphs.Nodes;

namespace LUIECompilerTests.Optimization;

[TestClass, TestCategory("Optimization")]
public class CircuitGraphTest
{
    public const string SimpleInput =
    @"
        qubit c;
        qubit a;
        h c;
        z c;
        x a;
    ";

    public readonly List<QubitTestPath> SimpleInputPaths = 
    [
        new()
        {
            Identifier = "id0",
            Path =
            [
                GateType.H,
                GateType.Z
            ]
        },
        new()
        {
            Identifier = "id1",
            Path =
            [
                GateType.X
            ]
        }
    ];
    public readonly List<QubitTestPath> SimpleInputZRemovedPaths = 
    [
        new()
        {
            Identifier = "id0",
            Path =
            [
                GateType.H
            ]
        },
        new()
        {
            Identifier = "id1",
            Path =
            [
                GateType.X
            ]
        }
    ];
    public const string SimpleRegister =
    @"
        qubit[2] c;
        h c[0];
        z c[0];
        x c[1];
    ";

    public readonly List<QubitTestPath> SimpleRegisterPaths = 
    [
        new()
        {
            Identifier = "id0",
            Index = 0,
            Path =
            [
                GateType.H,
                GateType.Z
            ]
        },
        new()
        {
            Identifier = "id0",
            Index = 1,
            Path =
            [
                GateType.X
            ]
        }
    ];
    
    public const string QFT =
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

    public const string UnusedQubits = @"
        qubit a;
        qubit b;
        qubit c;
        qubit d;

        h b;
        x d;
        z b;
        x d;
    ";

    public readonly List<QubitTestPath> QFTPaths = 
    [
        new()
        {
            Identifier = "id0",
            Index = 0,
            Path = 
            [
                GateType.H,
                GateType.P,
                GateType.P,
                GateType.P,
                GateType.P,
                GateType.X,
                GateType.X,
                GateType.X,
            ]
        },
        new()
        {
            Identifier = "id0",
            Index = 1,
            Path = 
            [
                GateType.P,
                GateType.H,
                GateType.P,
                GateType.P,
                GateType.P,
                GateType.X,
                GateType.X,
                GateType.X,
            ]
        },
        new()
        {
            Identifier = "id0",
            Index = 2,
            Path = 
            [
                GateType.P,
                GateType.P,
                GateType.H,
                GateType.P,
                GateType.P,
            ]
        },
        new()
        {
            Identifier = "id0",
            Index = 3,
            Path = 
            [
                GateType.P,
                GateType.P,
                GateType.P,
                GateType.H,
                GateType.P,
                GateType.X,
                GateType.X,
                GateType.X
            ]
        },
        new()
        {
            Identifier = "id0",
            Index = 4,
            Path = 
            [
                GateType.P,
                GateType.P,
                GateType.P,
                GateType.P,
                GateType.H,
                GateType.X,
                GateType.X,
                GateType.X
            ]
        }
    ];
    public readonly List<QubitTestPath> QFTFirstCXRemovedPaths = 
    [
        new()
        {
            Identifier = "id0",
            Index = 0,
            Path = 
            [
                GateType.H,
                GateType.P,
                GateType.P,
                GateType.P,
                GateType.P,
                GateType.X,
                GateType.X,
            ]
        },
        new()
        {
            Identifier = "id0",
            Index = 1,
            Path = 
            [
                GateType.P,
                GateType.H,
                GateType.P,
                GateType.P,
                GateType.P,
                GateType.X,
                GateType.X,
                GateType.X,
            ]
        },
        new()
        {
            Identifier = "id0",
            Index = 2,
            Path = 
            [
                GateType.P,
                GateType.P,
                GateType.H,
                GateType.P,
                GateType.P,
            ]
        },
        new()
        {
            Identifier = "id0",
            Index = 3,
            Path = 
            [
                GateType.P,
                GateType.P,
                GateType.P,
                GateType.H,
                GateType.P,
                GateType.X,
                GateType.X,
                GateType.X
            ]
        },
        new()
        {
            Identifier = "id0",
            Index = 4,
            Path = 
            [
                GateType.P,
                GateType.P,
                GateType.P,
                GateType.P,
                GateType.H,
                GateType.X,
                GateType.X
            ]
        }
    ];

    [TestMethod]
    public void SimpleInputTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(SimpleInput);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        CircuitGraph graph = new(program);

        CheckCircut(graph, SimpleInputPaths);
    }

    [TestMethod]
    public void NodeRemovalSimpleInputTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(SimpleInput);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        CircuitGraph graph = new(program);

        CheckCircut(graph, SimpleInputPaths);
        graph.Nodes.OfType<GateNode>().Single(n => n.Gate == GateType.Z).Remove();
        CheckCircut(graph, SimpleInputZRemovedPaths);
    }

    [TestMethod]
    public void SimpleRegisterTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(SimpleRegister);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        CircuitGraph graph = new(program);

        CheckCircut(graph, SimpleRegisterPaths);
    }

    [TestMethod]
    public void QFTCircuitTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(QFT);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        CircuitGraph graph = new(program);

        CheckCircut(graph, QFTPaths);
    }

    // Currently not working
    [TestMethod]
    public void RemovedGateQFTCircuitTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(QFT);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        CircuitGraph graph = new(program);

        // Formerly: Check for GateType.CX, but alls CX now replaced with controlled X gates for easier optimizations
        graph.Nodes.OfType<GateNode>().First(n => n.Gate == GateType.X && n.GetGuardQubits().Count == 1).Remove();

        CheckCircut(graph, QFTFirstCXRemovedPaths);
    }
    
    [TestMethod]
    public void QFTGraphTranslationTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(QFT);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        CircuitGraph graph = new(program);

        CheckCircut(graph, QFTPaths);

        QASMProgram translated = graph.ToQASM();
        
        CircuitGraph translatedGraph = new(translated);

        CheckCircut(translatedGraph, QFTPaths);

    }
    
    [TestMethod]
    public void UnusedQubitsTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(UnusedQubits);

        var codegen = new CodeGenerationListener();
        walker.Walk(codegen, parser.parse());

        QASMProgram program = codegen.CodeGen.GenerateCode();
        Assert.IsNotNull(program);

        CircuitGraph graph = new(program);

        Assert.AreEqual(4, graph.Qubits.Count);

        graph.RemoveUnusedQubits();

        Assert.AreEqual(2, graph.Qubits.Count);
        
        Assert.IsTrue(!graph.Qubits.Any(q => q.Identifier.Identifier == "id0"));
        Assert.IsTrue(!graph.Qubits.Any(q => q.Identifier.Identifier == "id2"));

        Assert.IsTrue(graph.Qubits.Any(q => q.Identifier.Identifier == "id1"));
        Assert.IsTrue(graph.Qubits.Any(q => q.Identifier.Identifier == "id3"));
    }


    
    public void CheckCircut(CircuitGraph graph, List<QubitTestPath> paths)
    {
        foreach (var path in paths)
        {
            GraphQubit qubit;
            if (path.Index != -1)
            {
                qubit = graph.GetGraphQubit(path.Identifier, path.Index);
            }
            else
            {
                qubit = graph.GetGraphQubit(path.Identifier);
            }
            CheckPath(qubit.GetPath(), path.Path);
        }
    }

    public void CheckPath(WirePath circuitPath, List<GateType> path)
    {
        IEnumerable<INode> nodes = circuitPath.InnerNodes;
        for(int i = 0; i < path.Count; i++)
        {
            var gateNode = nodes.ElementAt(i) as GateNode;
            Assert.IsNotNull(gateNode);
            Assert.AreEqual(path[i], gateNode.Gate);
        }
    }

    /// <summary>
    /// Tests that for each qubit there exists exactly one path.
    /// </summary>
    [TestMethod]
    public void EachQubitSinglePathTest()
    {
        var walker = Utils.GetWalker();
        var parser = Utils.GetParser(QFT);

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
            var outV = current.OutputEdges.OfType<CircuitEdge>();
            Assert.IsTrue(outV.Count(v => v.Qubit == qubit) == 1);
            current = outV.Single(v => v.Qubit == qubit).End;
        }
    }

    public class QubitTestPath
    {
        public required string Identifier { get; init; }
        public required List<GateType> Path { get; init; }

        public int Index { get; init; } = -1;
    }
}
