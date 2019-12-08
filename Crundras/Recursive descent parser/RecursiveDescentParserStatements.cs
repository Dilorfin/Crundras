using LexicalAnalyzer;

namespace Recursive_descent_parser
{
    public partial class RecursiveDescentParser
    {
        // Statement = InputStatement | OutputStatement | CompoundStatement | ExpressionStatement | SelectionStatement | IterationStatement | DeclarationStatement | AssignmentStatement.
        private SyntaxTreeNode Statement()
        {
            var node = GetTokenCode() switch
            {
                // 'identifier'
                1 => AssignmentStatement(),
                // '$'
                12 => InputStatement(),
                // '@'
                13 => OutputStatement(),
                // '{'
                29 => CompoundStatement(),
                // 'if'
                6 => SelectionStatement(),
                // 'for'
                7 => IterationStatement(),
                // 'int'
                4 => DeclarationStatement(),
                // 'float'
                5 => DeclarationStatement(),
                // default
                _ => ExpressionStatement()
            };

            return node;
        }

        // ExpressionStatement = Expression ';'.
        private SyntaxTreeNode ExpressionStatement()
        {
            SyntaxTreeNode node = Expression();
            ExpectedToken(31); // ';'
            return node;
        }

        // SelectionStatement = "if" '(' Expression ')' Statement.
        private SyntaxTreeNode SelectionStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("if");

            ExpectedToken(6); // 'if'
            ExpectedToken(27); // '('
            node.AddChild(Expression());
            ExpectedToken(28); // ')'
            node.AddChild(Statement());

            return node;
        }

        // IterationStatement = "for" AssignmentExpression "to" Expression "by" Expression "while" '(' Expression ')' Statement "rof" ';'.
        private SyntaxTreeNode IterationStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("for");

            ExpectedToken(7); // 'for'
            node.AddChild(AssignmentExpression());
            ExpectedToken(8); // 'to'
            node.AddChild(Expression());
            ExpectedToken(9); // 'by'
            node.AddChild(Expression());
            ExpectedToken(10); // 'while'
            ExpectedToken(27); // '('
            node.AddChild(Expression());
            ExpectedToken(28); // ')'
            node.AddChild(Statement());
            ExpectedToken(11); // 'rof'
            ExpectedToken(31); // ';'

            return node;
        }

        // CompoundStatement = '{' {Statement} '}'.
        private SyntaxTreeNode CompoundStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("statements");
            ExpectedToken(29); // '{'
            while (GetTokenCode() != 30)
            {
                node.AddChild(Statement());
            }
            ExpectedToken(30); // '}'
            return node;
        }

        // DeclarationStatement = ("int" | "float") Identifier ';'.
        private SyntaxTreeNode DeclarationStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("declaration");

            //"int" | "float"
            var token = ExpectedOneOfTokens("TypeSpecifier", 4, 5);
            node.AddChild(new SyntaxTreeNode(TokenTable.GetLexemeName(token.Code)));
            // identifier
            token = ExpectedToken(1);
            node.AddChild(new SyntaxTreeNode("identifier", token.ForeignId));

            ExpectedToken(31); // ';'

            return node;
        }

        // InputStatement = '$' Identifier ';'.
        private SyntaxTreeNode InputStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("input");

            ExpectedToken(12); // '$'
            var token = ExpectedToken(1); // identifier
            node.AddChild(new SyntaxTreeNode("identifier", token.ForeignId));
            ExpectedToken(31); // ';'

            return node;
        }

        // OutputStatement = '@' Expression ';'.
        private SyntaxTreeNode OutputStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("output");

            ExpectedToken(13); // '@'
            node.AddChild(Expression());
            ExpectedToken(31); // ';'

            return node;
        }

        // AssignmentStatement = AssignmentExpression ';'.
        private SyntaxTreeNode AssignmentStatement()
        {
            SyntaxTreeNode node = AssignmentExpression();
            ExpectedToken(31); // ';'
            return node;
        }
    }
}