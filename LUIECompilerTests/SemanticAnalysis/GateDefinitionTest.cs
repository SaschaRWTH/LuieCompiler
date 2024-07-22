using LUIECompiler.SemanticAnalysis;

namespace LUIECompilerTests.SemanticAnalysis
{
    
    [TestClass, TestCategory("SemanticAnalysis"), TestCategory("GateDefinition")]
    public class GateDefinitionTest
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

        public const string UseOfUndefined =
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

            Console.WriteLine(error.ToString());

            Assert.IsFalse(error.ContainsCriticalError);
        }
        [TestMethod]
        public void UseOfUndefinedTest()
        {
            var walker = Utils.GetWalker();
            var parser = Utils.GetParser(UseOfUndefined);
            var analysis = new DeclarationAnalysisListener();
            walker.Walk(analysis, parser.parse());
            var error = analysis.Error;

            Assert.IsTrue(error.ContainsCriticalError);

            Assert.IsTrue(error.Errors.Count == 1);
            Assert.IsTrue(error.CriticalErrors.Exists(e => e.ErrorContext.Line == 3));
        }
    }
}