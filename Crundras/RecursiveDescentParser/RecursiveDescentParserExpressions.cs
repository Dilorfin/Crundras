using Crundras.Common;
using Crundras.Common.Tables;
using System;
using System.Collections.Generic;

namespace RecursiveDescentParser
{
    public partial class RecursiveDescentParser
    {
        // Expression = [Sign] ( '(' Expression ')' | Literal | Identifier ) { Operator [Sign] ( '(' Expression ')' | Literal | Identifier ) }.
        private LinkedList<SyntaxTreeNode> Expression()
        {
            LinkedList<SyntaxTreeNode> node = new LinkedList<SyntaxTreeNode>();

            if (TokenCode == LexemesTable.GetLexemeId("+") || TokenCode == LexemesTable.GetLexemeId("-"))
            {
                node.AddLast(new SyntaxTreeNode(TokenCode));
                TransitToNextToken();
            }

            if (TokenCode == LexemesTable.GetLexemeId("("))
            {
                node.AddLast(new SyntaxTreeNode(TokenCode));
                TransitToNextToken();

                node.Last?.Value.AddChildren(Expression());

                ExpectedToken(LexemesTable.GetLexemeId(")"));
                node.AddLast(new SyntaxTreeNode(TokenCode));
            }
            else if (Token.IsIdentifierOrLiteral(TokenCode))
            {
                node.AddLast(new SyntaxTreeNode(tokenListNode.Value));

                TransitToNextToken();
            }
            else
            {
                var token = tokenListNode.Value;
                throw new Exception($"Unexpected token {LexemesTable.GetLexemeName(token.Code)} in line {token.Line}.");
            }

            while (TokenCode >= 15 && TokenCode <= 26)
            {
                node.AddLast(new SyntaxTreeNode(TokenCode));
                TransitToNextToken();

                if (TokenCode == LexemesTable.GetLexemeId("+") || TokenCode == LexemesTable.GetLexemeId("-"))
                {
                    node.AddLast(new SyntaxTreeNode(TokenCode));
                    TransitToNextToken();
                }

                if (TokenCode == LexemesTable.GetLexemeId("("))
                {
                    node.AddLast(new SyntaxTreeNode(TokenCode));
                    TransitToNextToken();

                    node.Last?.Value.AddChildren(Expression());

                    ExpectedToken(LexemesTable.GetLexemeId(")"));
                    node.AddLast(new SyntaxTreeNode(TokenCode));
                }
                else if (Token.IsIdentifierOrLiteral(TokenCode))
                {
                    node.AddLast(new SyntaxTreeNode(tokenListNode.Value));

                    TransitToNextToken();
                }
                else
                {
                    throw new Exception($"Unexpected token {LexemesTable.GetLexemeName(TokenCode)} in line {tokenListNode.Value.Line}.");
                }
            }

            return node;
        }

        // AssignmentExpression = Identifier '=' Expression.
        private SyntaxTreeNode AssignmentExpression()
        {
            SyntaxTreeNode node = new SyntaxTreeNode(tokenListNode.Value);

            ExpectedToken(LexemesTable.GetLexemeId("identifier"));

            node.AddChild(new SyntaxTreeNode(TokenCode));
            ExpectedToken(LexemesTable.GetLexemeId("="));
            node.Children[^1].AddChildren(Expression());

            return node;
        }
    }
}
