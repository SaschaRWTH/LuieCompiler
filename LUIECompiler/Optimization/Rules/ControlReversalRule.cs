using LUIECompiler.Common;
using LUIECompiler.Optimization.Graphs;
using LUIECompiler.Optimization.Graphs.Nodes;

namespace LUIECompiler.Optimization.Rules
{
    public class ControlReversalRule : OptimizationRule
    {
        public override int MaxRuleDepth => 1;

        public static readonly ControlReversalRule Rule = new();

        public override void Apply(WirePath path)
        {
            throw new NotImplementedException();
        }

        public override bool IsApplicable(WirePath path)
        {
            if (path.Length != 1)
            {
                return false;
            }

            if (path.Start is not GateNode node)
            {
                return false;
            }

            if (node.Gate != GateType.X && node.Gate != GateType.CX)
            {
                return false;
            }
            

            throw new NotImplementedException();
        }
    }
}