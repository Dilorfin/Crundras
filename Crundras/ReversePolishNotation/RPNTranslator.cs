using Crundras.Common;
using Crundras.Common.Tables;
using System;
using System.Collections.Generic;

namespace ReversePolishNotation
{
    public class LabelsTable : Dictionary<uint, uint>
    {
    }

    public class RPNTranslator
    {
        private readonly Stack<RPNToken> stack = new Stack<RPNToken>();
        private readonly LinkedList<RPNToken> result = new LinkedList<RPNToken>();
        private readonly TablesCollection tables;
        private readonly LabelsTable labelsTable;

        private RPNTranslator(TablesCollection tables)
        {
            this.tables = tables;
            this.labelsTable = new LabelsTable();
        }

        private void ArithmeticExpression(List<SyntaxTreeNode> nodes)
        {
            if (nodes.Count == 0) return;

            var priorityTable = new Dictionary<string, int>
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
                { "%", 3 },
                { "NEG", 4 },
                { "**", 5 }
            };

            for (int i = 0; i < nodes.Count; i++)
            {
                if (Token.IsIdentifierOrLiteral(nodes[i].LexemeCode))
                {
                    result.AddLast(new RPNToken(nodes[i]));
                    continue;
                }

                if (nodes[i].LexemeCode == LexemesTable.GetLexemeId("("))
                {
                    stack.Push(new RPNToken(nodes[i]));
                }
                else if (nodes[i].LexemeCode == LexemesTable.GetLexemeId(")"))
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

                    if ((i == 0) || (nodes[i - 1].LexemeCode != LexemesTable.GetLexemeId(")")
                                     && !Token.IsIdentifierOrLiteral(nodes[i - 1].LexemeCode)))
                    {
                        if (nodes[i].LexemeCode == LexemesTable.GetLexemeId("-"))
                        {
                            token.Name = "NEG";
                        }
                        else if (nodes[i].LexemeCode == LexemesTable.GetLexemeId("+"))
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

        private void SetIdentifierType(SyntaxTreeNode identifierNode, uint type)
        {
            if (!identifierNode.Id.HasValue)
            {
                throw new Exception("Expected identifier.");
            }

            var identifier = tables.IdentifiersTable[identifierNode.Id.Value];
            identifier.Type = type;
        }

        private void IfStatement(SyntaxTreeNode node)
        {
            var labelId = (uint)(labelsTable.Count + 1);
            result.AddLast(new RPNToken(0, $"LB{labelId}", labelId));
            ArithmeticExpression(node.Children[0].Children);
            result.AddLast(new RPNToken(LexemesTable.GetLexemeId("if")));
            Statement(node.Children[1]);
            labelsTable[labelId] = (uint)result.Count;
            result.AddLast(new RPNToken(0, $"LB{labelId}", labelId));
        }

        private void ForStatement(SyntaxTreeNode node)
        {
            // stm 1
            ArithmeticExpression(node.Children[1].Children);
            result.AddLast(new RPNToken(node.Children[0]));
            result.AddLast(new RPNToken(LexemesTable.GetLexemeId("=")));

            var startLabelId = (uint)(labelsTable.Count + 1);
            labelsTable[startLabelId] = (uint)result.Count;
            result.AddLast(new RPNToken(0, $"LB{startLabelId}", startLabelId));

            var endLabelId = (uint)(labelsTable.Count + 1);
            // stm 4
            result.AddLast(new RPNToken(0, $"LB{endLabelId}", endLabelId));
            ArithmeticExpression(node.Children[5].Children);
            result.AddLast(new RPNToken(LexemesTable.GetLexemeId("if")));

            // stm 2
            result.AddLast(new RPNToken(0, $"LB{endLabelId}", endLabelId));
            ArithmeticExpression(node.Children[2].Children);
            result.AddLast(new RPNToken(node.Children[0]));
            result.AddLast(new RPNToken(LexemesTable.GetLexemeId("==")));
            result.AddLast(new RPNToken(LexemesTable.GetLexemeId("if")));

            Statement(node.Children[6]);

            // stm 3
            ArithmeticExpression(node.Children[3].Children);
            result.AddLast(new RPNToken(node.Children[0]));
            result.AddLast(new RPNToken(LexemesTable.GetLexemeId("+")));
            result.AddLast(new RPNToken(node.Children[0]));
            result.AddLast(new RPNToken(LexemesTable.GetLexemeId("=")));

            // goto start
            result.AddLast(new RPNToken(0, $"LB{startLabelId}", startLabelId));
            result.AddLast(new RPNToken(LexemesTable.GetLexemeId("goto")));
            // end
            result.AddLast(new RPNToken(0, $"LB{endLabelId}", endLabelId));
        }

        private void Statement(SyntaxTreeNode node)
        {
            switch (node.LexemeCode)
            {
                // 'identifier'
                case 1:
                    ArithmeticExpression(node.Children[0].Children);
                    result.AddLast(new RPNToken(node));
                    result.AddLast(new RPNToken(LexemesTable.GetLexemeId("=")));
                    break;
                // '$'
                case 12:
                    result.AddLast(new RPNToken(node.Children[0]));
                    result.AddLast(new RPNToken(LexemesTable.GetLexemeId("$")));
                    break;
                // '@'
                case 13:
                    ArithmeticExpression(node.Children);
                    result.AddLast(new RPNToken(LexemesTable.GetLexemeId("@")));
                    break;
                // '{'
                case 29:
                    Statements(node);
                    break;
                // 'if'
                case 6:
                    IfStatement(node);
                    break;
                // 'for'
                case 7:
                    ForStatement(node);
                    break;
                // 'int'
                case 4:
                    SetIdentifierType(node.Children[0], 2);
                    break;
                // 'float'
                case 5:
                    SetIdentifierType(node.Children[0], 3);
                    break;
            }
        }

        private void Statements(SyntaxTreeNode node)
        {
            foreach (var child in node.Children)
            {
                Statement(child);
                stack.Clear();
            }
        }

        public static LinkedList<RPNToken> Analyze(TablesCollection tables, SyntaxTreeNode root)
        {
            var syntaxAnalyzerRpn = new RPNTranslator(tables);
            syntaxAnalyzerRpn.Statements(root);
            return syntaxAnalyzerRpn.result;
        }
    }
}