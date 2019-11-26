using System;
using LexicalAnalyzer;

namespace Recursive_descent_parser
{
    public partial class RecursiveDescentParser
    {
        // Expression = [Sign] ( '(' Expression ')' | Literal | Identifier ) { Operator [Sign] ( '(' Expression ')' | Literal | Identifier ) }.
        SyntaxTreeNode Expression()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("expression");
            
            if (GetTokenCode() == 15) // '+'
            {
                node.AddChild(new SyntaxTreeNode("+"));
                TransitToNextToken();
            }
            else if (GetTokenCode() == 16) // '-'
            {
                node.AddChild(new SyntaxTreeNode("-"));
                TransitToNextToken();
            }

            if (GetTokenCode() == 27) // '('
            {
                node.AddChild(new SyntaxTreeNode("("));
                TransitToNextToken();

                node.AddChild(Expression());

                ExpectedToken(28); // ')'
                node.AddChild(new SyntaxTreeNode(")"));
            }
            else if (GetTokenCode() >= 1 && GetTokenCode() <= 3)
            {
                var name = TokenTable.GetLexemeName(GetTokenCode());
                node.AddChild(new SyntaxTreeNode(name, tokenListNode.Value.id));

                TransitToNextToken();
            }
            else
            {
                var token = tokenListNode.Value;
                throw new Exception($"Unexpected token {TokenTable.GetLexemeName(token.code)} in line {token.line}.");
            }

            while(GetTokenCode() >= 15 && GetTokenCode() <= 26)
            {
                node.AddChild(new SyntaxTreeNode(TokenTable.GetLexemeName(GetTokenCode())));
                TransitToNextToken();

                if (GetTokenCode() == 15) // '+'
                {
                    node.AddChild(new SyntaxTreeNode("+"));
                    TransitToNextToken();
                }
                else if (GetTokenCode() == 16) // '-'
                {
                    node.AddChild(new SyntaxTreeNode("-"));
                    TransitToNextToken();
                }

                if (GetTokenCode() == 27) // '('
                {
                    node.AddChild(new SyntaxTreeNode("("));
                    TransitToNextToken();

                    node.AddChild(Expression());

                    ExpectedToken(28); // ')'
                    node.AddChild(new SyntaxTreeNode(")"));
                }
                else if (GetTokenCode() >= 1 && GetTokenCode() <= 3)
                {
                    var name = TokenTable.GetLexemeName(GetTokenCode());
                    node.AddChild(new SyntaxTreeNode(name, tokenListNode.Value.id));

                    TransitToNextToken();
                }
                else
                {
                    var token = tokenListNode.Value;
                    throw new Exception($"Unexpected token {TokenTable.GetLexemeName(token.code)} in line {token.line}.");
                }
            }

            return node;
        }

        // AssignmentExpression = Identifier '=' Expression.
        SyntaxTreeNode AssignmentExpression()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("assignment");

            var token = ExpectedToken(1); // identifier
            node.AddChild(new SyntaxTreeNode("identifier", token.id));

            ExpectedToken(14); // '='

            node.AddChild(Expression());

            return node;
        }
    }
}
