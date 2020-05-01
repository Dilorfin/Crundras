using Crundras.Common;
using System;

namespace RecursiveDescentParser
{
    public partial class RecursiveDescentParser
    {
        // Expression = [Sign] ( '(' Expression ')' | Literal | Identifier ) { Operator [Sign] ( '(' Expression ')' | Literal | Identifier ) }.
        private SyntaxTreeNode Expression()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("expression");

            if (GetTokenCode() == TokenTable.GetLexemeId("+"))
            {
                node.AddChild(new SyntaxTreeNode("+"));
                TransitToNextToken();
            }
            else if (GetTokenCode() == TokenTable.GetLexemeId("-"))
            {
                node.AddChild(new SyntaxTreeNode("-"));
                TransitToNextToken();
            }

            if (GetTokenCode() == TokenTable.GetLexemeId("("))
            {
                node.AddChild(new SyntaxTreeNode("("));
                TransitToNextToken();

                node.AddChild(Expression());

                ExpectedToken(TokenTable.GetLexemeId(")"));
                node.AddChild(new SyntaxTreeNode(")"));
            }
            else if (GetTokenCode() >= 1 && GetTokenCode() <= 3)
            {
                var name = TokenTable.GetLexemeName(GetTokenCode());
                node.AddChild(new SyntaxTreeNode(name, tokenListNode.Value.ForeignId));

                TransitToNextToken();
            }
            else
            {
                var token = tokenListNode.Value;
                throw new Exception($"Unexpected token {TokenTable.GetLexemeName(token.Code)} in line {token.Line}.");
            }

            while (GetTokenCode() >= 15 && GetTokenCode() <= 26)
            {
                node.AddChild(new SyntaxTreeNode(TokenTable.GetLexemeName(GetTokenCode())));
                TransitToNextToken();

                if (GetTokenCode() == TokenTable.GetLexemeId("+"))
                {
                    node.AddChild(new SyntaxTreeNode("+"));
                    TransitToNextToken();
                }
                else if (GetTokenCode() == TokenTable.GetLexemeId("-"))
                {
                    node.AddChild(new SyntaxTreeNode("-"));
                    TransitToNextToken();
                }

                if (GetTokenCode() == TokenTable.GetLexemeId("("))
                {
                    node.AddChild(new SyntaxTreeNode("("));
                    TransitToNextToken();

                    node.AddChild(Expression());

                    ExpectedToken(TokenTable.GetLexemeId(")"));
                    node.AddChild(new SyntaxTreeNode(")"));
                }
                else if (GetTokenCode() >= 1 && GetTokenCode() <= 3) // identifier && int/float literals
                {
                    var name = TokenTable.GetLexemeName(GetTokenCode());
                    node.AddChild(new SyntaxTreeNode(name, tokenListNode.Value.ForeignId));

                    TransitToNextToken();
                }
                else
                {
                    var token = tokenListNode.Value;
                    throw new Exception($"Unexpected token {TokenTable.GetLexemeName(token.Code)} in line {token.Line}.");
                }
            }

            return node;
        }

        // AssignmentExpression = Identifier '=' Expression.
        private SyntaxTreeNode AssignmentExpression()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("assignment");

            var token = ExpectedToken(TokenTable.GetLexemeId("identifier"));
            node.AddChild(new SyntaxTreeNode("identifier", token.ForeignId));

            ExpectedToken(TokenTable.GetLexemeId("="));

            node.AddChild(Expression());

            return node;
        }
    }
}
