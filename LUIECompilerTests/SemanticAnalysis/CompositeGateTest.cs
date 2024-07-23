using LUIECompiler.SemanticAnalysis;

namespace LUIECompilerTests.SemanticAnalysis
{
    
    [TestClass, TestCategory("SemanticAnalysis"), TestCategory("CompositeGate")]
    public class CompositeGateTest
    {
        public const string SimpleGateDefinition =
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

        public const string UseOfUndefinedParameter =
            "gate swap(a, b) do\n" +
            "    cx a, b;\n" +
            "    cx c, a;\n" +
            "    cx a, b;\n" +
            "end\n" +
            "\n" +
            "qubit b;\n" +
            "qubit c;\n" +
            "x c;\n" +
            "swap b, c;";

        public const string UseOfUndefinedGate =
            "gate swap2(a, b) do\n" +
            "    cx a, b;\n" +
            "    cx b, a;\n" +
            "    cx a, b;\n" +
            "end\n" +
            "\n" +
            "qubit b;\n" +
            "qubit c;\n" +
            "x c;\n" +
            "swap b, c;";

        public const string WrongUseOfGateIdentifier =
            "gate swap(a, b) do\n" +
            "    cx a, b;\n" +
            "    cx b, a;\n" +
            "    cx a, b;\n" +
            "end\n" +
            "\n" +
            "qubit[sizeof(swap)] b;\n" +
            "qubit c;\n" +
            "x swap;";

        public const string GateUseInGateDefinition =
            "gate swap(a, b) do\n" +
            "    cx a, b;\n" +
            "    cx b, a;\n" +
            "    cx a, b;\n" +
            "gate swap1(a, b) do\n" +
            "    swap a, b;\n" +
            "end\n" +
            "\n" +
            "qubit b;\n" +
            "qubit c;\n" +
            "x c;\n" +
            "swap b, c;";

        [TestMethod]
        public void SimpleGateDefinitionTest()
        {
            var walker = Utils.GetWalker();
            var parser = Utils.GetParser(SimpleGateDefinition);
            var analysis = new DeclarationAnalysisListener();
            walker.Walk(analysis, parser.parse());
            var error = analysis.Error;

            Assert.IsFalse(error.ContainsCriticalError);
        }

        [TestMethod]
        public void SimpleGateDefinitionTypeCheckingTest()
        {
            var walker = Utils.GetWalker();
            var parser = Utils.GetParser(SimpleGateDefinition);
            var analysis = new TypeCheckListener();
            walker.Walk(analysis, parser.parse());
            var error = analysis.Error;

            Assert.IsFalse(error.ContainsCriticalError);
        }

        [TestMethod]
        public void UseOfUndefinedParameterTest()
        {
            var walker = Utils.GetWalker();
            var parser = Utils.GetParser(UseOfUndefinedParameter);
            var analysis = new DeclarationAnalysisListener();
            walker.Walk(analysis, parser.parse());
            var error = analysis.Error;

            Assert.IsTrue(error.ContainsCriticalError);

            Assert.IsTrue(error.Errors.Count == 1);
            Assert.IsTrue(error.CriticalErrors.Exists(e => e.ErrorContext.Line == 3));
        }

        [TestMethod]
        public void UseOfUndefinedGateTest()
        {
            var walker = Utils.GetWalker();
            var parser = Utils.GetParser(UseOfUndefinedGate);
            var analysis = new DeclarationAnalysisListener();
            walker.Walk(analysis, parser.parse());
            var error = analysis.Error;

            Assert.IsTrue(error.ContainsCriticalError);

            Assert.IsTrue(error.Errors.Count == 1);
            Assert.IsTrue(error.CriticalErrors.Exists(e => e.ErrorContext.Line == 10));
        }

        [TestMethod]
        public void WrongUseOfGateIdentifierTest()
        {
            var walker = Utils.GetWalker();
            var parser = Utils.GetParser(WrongUseOfGateIdentifier);
            var analysis = new TypeCheckListener();
            walker.Walk(analysis, parser.parse());
            var error = analysis.Error;

            Assert.IsTrue(error.ContainsCriticalError);

            Assert.IsTrue(error.CriticalErrors.Exists(e => e.ErrorContext.Line == 7));
            // TODO: "A critical Error occured at (9, 0)" occures twice
            Assert.IsTrue(error.CriticalErrors.Exists(e => e.ErrorContext.Line == 9));
        }


        [TestMethod]
        public void GateUseInGateDefinitionTest()
        {
            var walker = Utils.GetWalker();
            var parser = Utils.GetParser(GateUseInGateDefinition);
            var analysis = new TypeCheckListener();
            walker.Walk(analysis, parser.parse());
            var error = analysis.Error;

            Assert.IsFalse(error.ContainsCriticalError);
        }
    }
}