using Crundras.Common;
using Crundras.Common.Tables;
using System;
using System.Collections.Generic;

namespace RPNTranslator
{
    public class RPNTranslator
    {
        private readonly Stack<RPNToken> stack = new Stack<RPNToken>();
        private readonly LinkedList<RPNToken> result = new LinkedList<RPNToken>();
        private readonly TablesCollection tables;

        private RPNTranslator(TablesCollection tables)
        {
            this.tables = tables;
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
            var labelId = tables.LabelsTable.GetId($"_LB{tables.LabelsTable.Count + 1}");
            var label = tables.LabelsTable[labelId];
            var labelToken = new RPNToken(LexemesTable.GetLexemeId("label"), label.Name, labelId);

            result.AddLast(labelToken);

            ArithmeticExpression(node.Children[0].Children);
            result.AddLast(new RPNToken(LexemesTable.GetLexemeId("if")));

            Statement(node.Children[1].Children[0]);

            label.Position = (uint)result.Count;
            result.AddLast(labelToken);
        }

        private void ForStatement(SyntaxTreeNode node)
        {
            var endLabelId = tables.LabelsTable.GetId($"_LB{tables.LabelsTable.Count + 1}");
            var endLabel = tables.LabelsTable[endLabelId];
            var endLabelToken = new RPNToken(LexemesTable.GetLexemeId("label"), endLabel.Name, endLabelId);

            var startLabelId = tables.LabelsTable.GetId($"_LB{tables.LabelsTable.Count + 1}");
            var startLabelToken = new RPNToken(LexemesTable.GetLexemeId("label"), tables.LabelsTable[startLabelId].Name, startLabelId);

            // stm 1
            ArithmeticExpression(node.Children[1].Children);
            result.AddLast(new RPNToken(node.Children[0]));
            result.AddLast(new RPNToken(LexemesTable.GetLexemeId("=")));

            // start label
            tables.LabelsTable[startLabelId].Position = (uint)result.Count;
            result.AddLast(startLabelToken);

            // stm 4
            result.AddLast(endLabelToken);
            ArithmeticExpression(node.Children[5].Children);
            var id = tables.IntLiteralsTable.GetId(0);
            result.AddLast(new RPNToken(LexemesTable.GetLexemeId("int_literal"), "int_literal", id));
            result.AddLast(new RPNToken(LexemesTable.GetLexemeId("!="), "!="));
            result.AddLast(new RPNToken(LexemesTable.GetLexemeId("if")));

            // stm 2
            result.AddLast(endLabelToken);
            ArithmeticExpression(node.Children[2].Children);
            result.AddLast(new RPNToken(node.Children[0]));
            result.AddLast(new RPNToken(LexemesTable.GetLexemeId(">")));
            id = tables.IntLiteralsTable.GetId(0);
            result.AddLast(new RPNToken(LexemesTable.GetLexemeId("int_literal"), "int_literal", id));
            result.AddLast(new RPNToken(LexemesTable.GetLexemeId("!="), "!="));
            result.AddLast(new RPNToken(LexemesTable.GetLexemeId("if")));

            Statement(node.Children[6].Children[0]);

            // stm 3
            ArithmeticExpression(node.Children[3].Children);
            result.AddLast(new RPNToken(node.Children[0]));
            result.AddLast(new RPNToken(LexemesTable.GetLexemeId("+")));
            result.AddLast(new RPNToken(node.Children[0]));
            result.AddLast(new RPNToken(LexemesTable.GetLexemeId("=")));

            // goto start
            result.AddLast(startLabelToken);
            result.AddLast(new RPNToken(LexemesTable.GetLexemeId("goto")));
            // end
            endLabel.Position = (uint)result.Count;
            result.AddLast(endLabelToken);
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
                // 'label'
                case 2:
                    tables.LabelsTable[node.Id.Value].Position = (uint)result.Count;
                    result.AddLast(new RPNToken(node));
                    break;
                // '$'
                case 14:
                    result.AddLast(new RPNToken(node.Children[0]));
                    result.AddLast(new RPNToken(LexemesTable.GetLexemeId("$")));
                    break;
                // '@'
                case 15:
                    ArithmeticExpression(node.Children);
                    result.AddLast(new RPNToken(LexemesTable.GetLexemeId("@")));
                    break;
                // '{'
                case 31:
                    Statements(node);
                    break;
                // 'if'
                case 7:
                    IfStatement(node);
                    break;
                // 'for'
                case 8:
                    ForStatement(node);
                    break;
                // 'int'
                case 5:
                    SetIdentifierType(node.Children[0], LexemesTable.GetLexemeId("int_literal"));
                    break;
                // 'float'
                case 6:
                    SetIdentifierType(node.Children[0], LexemesTable.GetLexemeId("float_literal"));
                    break;
                // 'goto'
                case 13:
                    result.AddLast(new RPNToken(node.Children[0]));
                    result.AddLast(new RPNToken(LexemesTable.GetLexemeId("goto")));
                    break;
            }
        }

        private void Statements(SyntaxTreeNode node)
        {
            foreach (var child in node.Children)
            {
                Statement(child);
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