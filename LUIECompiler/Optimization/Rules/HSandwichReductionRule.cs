using LUIECompiler.CodeGeneration.Codes;
using LUIECompiler.CodeGeneration.Exceptions;
using LUIECompiler.Common;
using LUIECompiler.Optimization.Graphs;
using LUIECompiler.Optimization.Graphs.Nodes;

namespace LUIECompiler.Optimization.Rules
{
    /// <summary>
    /// Represents a gate reduction rule for gates sandwiched between to H gates.
    /// 
    /// E.g., the gate combination HXH is equivalent to Z.
    /// </summary>
    public class HSandwichReductionRule : OptimizationRule
    {
        public override int MaxRuleDepth => 3;

        public static readonly HSandwichReductionRule HXHRule = new(GateType.X, GateType.Z);
        public static readonly HSandwichReductionRule HZHRule = new(GateType.Z, GateType.X);

        public static readonly List<HSandwichReductionRule> GateReductionRules =
        [
            HXHRule,
            HZHRule
        ];

        public GateType SandwichedGate { get; }

        public GateType EquivalentGate { get; }

        public HSandwichReductionRule(GateType sandwichedGate, GateType equivalentGate)
        {
            SandwichedGate = sandwichedGate;
            EquivalentGate = equivalentGate;
        }

        public override void Apply(WirePath path)
        {
            if (path.Length != 3)
            {
                throw new InternalException()
                {
                    Reason = "The path must have a length of 3."
                };
            }

            if (path.Start is not GateNode start || path.End is not GateNode end)
            {
                throw new InternalException()
                {
                    Reason = "Start and/or end nodes are not gate nodes."
                };
            }

            if (path.Nodes[1] is not GateNode sandwiched)
            {
                throw new InternalException()
                {
                    Reason = "Sandwiched node is not a gate."
                };
            }

            start.Remove();
            end.Remove();

            GateApplicationCode newGate = new
            (
                gate: new GateCode(EquivalentGate),
                arguments: [ ..sandwiched.GateCode.Arguments],
                guards: [ ..sandwiched.GateCode.Guards]
            );

            sandwiched.ReplaceGate(newGate);
        }

        public override bool IsApplicable(WirePath path)
        {
            if (path.Length != 3)
            {
                return false;
            }

            if (path.Start is not GateNode start || path.End is not GateNode end)
            {
                return false;
            }

            if (start.Gate != GateType.H || end.Gate != GateType.H)
            {
                return false;
            }

            if (path.Nodes[1] is not GateNode sandwiched || sandwiched.Gate != SandwichedGate)
            {
                return false;
            }

            if (start.GateCode.Guards.Count == 0 && end.GateCode.Guards.Count == 0)
            {
                return true;
            }

            return OperateOnSameQubit(path) && ConsecutiveGatesForAllQubits(path);
        }
    }
}