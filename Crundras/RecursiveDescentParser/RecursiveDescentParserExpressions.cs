using Crundras.Common;
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

            if (TokenCode == TokenTable.GetLexemeId("+") || TokenCode == TokenTable.GetLexemeId("-"))
            {
                node.AddLast(new SyntaxTreeNode(TokenCode));
                TransitToNextToken();
            }

            if (TokenCode == TokenTable.GetLexemeId("("))
            {
                node.AddLast(new SyntaxTreeNode(TokenCode));
                TransitToNextToken();

                node.Last?.Value.AddChildren(Expression());

                ExpectedToken(TokenTable.GetLexemeId(")"));
                node.AddLast(new SyntaxTreeNode(TokenCode));
            }
            else if (TokenCode >= 1 && TokenCode <= 3)
            {
                node.AddLast(new SyntaxTreeNode(tokenListNode.Value));

                TransitToNextToken();
            }
            else
            {
                var token = tokenListNode.Value;
                throw new Exception($"Unexpected token {TokenTable.GetLexemeName(token.Code)} in line {token.Line}.");
            }

            while (TokenCode >= 15 && TokenCode <= 26)
            {
                node.AddLast(new SyntaxTreeNode(TokenCode));
                TransitToNextToken();

                if (TokenCode == TokenTable.GetLexemeId("+") || TokenCode == TokenTable.GetLexemeId("-"))
                {
                    node.AddLast(new SyntaxTreeNode(TokenCode));
                    TransitToNextToken();
                }

                if (TokenCode == TokenTable.GetLexemeId("("))
                {
                    node.AddLast(new SyntaxTreeNode(TokenCode));
                    TransitToNextToken();

                    node.Last?.Value.AddChildren(Expression());

                    ExpectedToken(TokenTable.GetLexemeId(")"));
                    node.AddLast(new SyntaxTreeNode(TokenCode));
                }
                else if (TokenCode >= 1 && TokenCode <= 3) // identifier && int/float literals
                {
                    node.AddLast(new SyntaxTreeNode(tokenListNode.Value));

                    TransitToNextToken();
                }
                else
                {
                    throw new Exception($"Unexpected token {TokenTable.GetLexemeName(TokenCode)} in line {tokenListNode.Value.Line}.");
                }
            }

            return node;
        }

        // AssignmentExpression = Identifier '=' Expression.
        private SyntaxTreeNode AssignmentExpression()
        {
            SyntaxTreeNode node = new SyntaxTreeNode(tokenListNode.Value);

            ExpectedToken(TokenTable.GetLexemeId("identifier"));
            
            node.AddChild(new SyntaxTreeNode(TokenCode));
            ExpectedToken(TokenTable.GetLexemeId("="));
            node.Children[^1].AddChildren(Expression());

            return node;
        }
    }
}
