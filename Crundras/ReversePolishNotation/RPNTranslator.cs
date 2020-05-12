using Crundras.Common;
using System.Collections.Generic;
using System.Linq;

namespace ReversePolishNotation
{
    class RPNToken
    {
        public string Name { get; set; }

        public uint LexemeCode { get; }

        public uint? Id { get; }

        public RPNToken(uint lexemeCode, string name, uint? id = null)
        {
            Name = name;
            LexemeCode = lexemeCode;
            Id = id;
        }

        public RPNToken(uint lexemeCode, uint? id = null)
        {
            Name = TokenTable.GetLexemeName(lexemeCode);
            LexemeCode = lexemeCode;
            Id = id;
        }
        public RPNToken(SyntaxTreeNode treeNode)
        {
            Name = TokenTable.GetLexemeName(treeNode.LexemeCode);
            LexemeCode = treeNode.LexemeCode;
            Id = treeNode.Id;
        }
    }

    internal class RPNTranslator
    {
        private readonly Dictionary<string, int> priorityTable = new Dictionary<string, int>
        {
            { "(", 0 },
            { ")", 0 },
            { "<", 1 },
            { "<=", 1 },
            { ">", 1 },
            { ">=", 1 },
            { "==", 1 },
            { "!=", 1 },
            { "+", 2 },
            { "-", 2 },
            { "*", 3 },
            { "/", 3 },
            { "NEG", 5 },
            { "**", 4 }
        };

        // may be stack should be local for arithmetic expression
        private readonly Stack<RPNToken> stack = new Stack<RPNToken>();
        private readonly LinkedList<RPNToken> result = new LinkedList<RPNToken>();

        private void ArithmeticExpression(List<SyntaxTreeNode> nodes)
        {
            if (nodes.Count == 0) return;

            for (int i = 0; i < nodes.Count; i++)
            {
                if (Token.IsIdentifierOrLiteral(nodes[i].LexemeCode))
                {
                    result.AddLast(new RPNToken(nodes[i]));
                    continue;
                }

                if (nodes[i].LexemeCode == TokenTable.GetLexemeId("("))
                {
                    stack.Push(new RPNToken(nodes[i]));
                }
                else if (nodes[i].LexemeCode == TokenTable.GetLexemeId(")"))
                {
                    var token = stack.Pop();
                    while (token.Name != "(")
                    {
                        result.AddLast(token);
                        token = stack.Pop();
                    }
                }
                else
                {
                    var token = new RPNToken(nodes[i]);

                    if ((i == 0) || (nodes[i - 1].LexemeCode != TokenTable.GetLexemeId(")")
                                     && !Token.IsIdentifierOrLiteral(nodes[i - 1].LexemeCode)))
                    {
                        if (nodes[i].LexemeCode == TokenTable.GetLexemeId("-"))
                        {
                            token.Name = "NEG";
                        }
                        else if (nodes[i].LexemeCode == TokenTable.GetLexemeId("+"))
                        {
                            continue;
                        }
                    }
                    while ((stack.Count > 0) && priorityTable[token.Name] <= priorityTable[stack.Peek().Name])
                    {
                        result.AddLast(stack.Pop());
                    }

                    stack.Push(token);
                }
            }

            while (stack.Count > 0)
            {
                result.AddLast(stack.Pop());
            }
        }

        private void Root(SyntaxTreeNode root)
        {

        }

        public static LinkedList<RPNToken> Analyze(SyntaxTreeNode root)
        {
            var syntaxAnalyzerRpn = new RPNTranslator();
            syntaxAnalyzerRpn.ArithmeticExpression(root.Children[0].Children);
            return syntaxAnalyzerRpn.result;
        }
    }
}