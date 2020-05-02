using Crundras.Common;
using System.Collections.Generic;

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
            { ")", 1 },
            { "<", 2 },
            { "<=", 2 },
            { ">", 2 },
            { ">=", 2 },
            { "==", 2 },
            { "!=", 2 },
            { "+", 3 },
            { "-", 3 },
            { "*", 4 },
            { "/", 4 },
            { "NEG", 4 },
            { "**", 5 }
        };

        // stack should be local for arithmetic expression
        private readonly Stack<RPNToken> stack = new Stack<RPNToken>();
        private readonly LinkedList<RPNToken> result = new LinkedList<RPNToken>();

        private void RPNArithmeticExpression(List<SyntaxTreeNode> nodes)
        {
            if(nodes.Count == 0) return;

            if (nodes[0].LexemeCode >= 1 && nodes[0].LexemeCode <= 3)
            {
                result.AddLast(new RPNToken(nodes[0]));
            }
            else if(nodes[0].LexemeCode != TokenTable.GetLexemeId("+"))
            {
                var token = new RPNToken(nodes[0]);
                if (nodes[0].LexemeCode == TokenTable.GetLexemeId("-"))
                {
                    token.Name = "NEG";
                }

                while ((stack.Count > 0) && priorityTable[token.Name] <= priorityTable[stack.Peek().Name])
                {
                    result.AddLast(stack.Pop());
                }
                stack.Push(token);
            }

            for (int i = 1; i < nodes.Count; i++)
            {
                if (nodes[i].LexemeCode >= 1 && nodes[i].LexemeCode <= 3)
                {
                    result.AddLast(new RPNToken(nodes[i]));
                }
                else
                {
                    // CHECK: if Token class should contain "IsIdentifier" & "IsLiteral" functions
                    var token = new RPNToken(nodes[i]);
                    if (nodes[i-1].LexemeCode != TokenTable.GetLexemeId(")")
                        && nodes[i-1].LexemeCode != TokenTable.GetLexemeId("identifier")
                        && nodes[i-1].LexemeCode != TokenTable.GetLexemeId("int_literal")
                        && nodes[i-1].LexemeCode != TokenTable.GetLexemeId("float_literal"))
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

        public static LinkedList<RPNToken> Analyze(SyntaxTreeNode root)
        {
            var syntaxAnalyzerRpn = new RPNTranslator();
            syntaxAnalyzerRpn.RPNArithmeticExpression(root.Children[0].Children);
            return syntaxAnalyzerRpn.result;
        }
    }
}