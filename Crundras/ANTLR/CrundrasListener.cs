using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Crundras;
using System;

namespace ANTLR
{
    public class CrundrasListener : CrundrasBaseListener
    {
        public override void EnterEveryRule([NotNull] ParserRuleContext context)
        {
            Console.Write("EnterRule: ");
            Console.WriteLine(CrundrasParser.ruleNames[context.RuleIndex]);
        }

        public override void ExitEveryRule([NotNull] ParserRuleContext context)
        {
            Console.Write("ExitRule: ");
            Console.WriteLine(CrundrasParser.ruleNames[context.RuleIndex]);
        }

        public override void VisitTerminal([NotNull] ITerminalNode node)
        {
            Console.Write(CrundrasParser.DefaultVocabulary.GetDisplayName(node.Symbol.Type));
            Console.Write(" ");
            Console.WriteLine(node.GetText());
        }

        public override void VisitErrorNode([NotNull] IErrorNode node)
        {

        }
    }
}