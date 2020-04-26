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
            ExpectedToken(TokenTable.GetLexemeId(";"));
            return node;
        }

        // SelectionStatement = "if" '(' Expression ')' Statement.
        private SyntaxTreeNode SelectionStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("if");

            ExpectedToken(TokenTable.GetLexemeId("if"));
            ExpectedToken(TokenTable.GetLexemeId("("));
            node.AddChild(Expression());
            ExpectedToken(TokenTable.GetLexemeId(")"));
            node.AddChild(Statement());

            return node;
        }

        // IterationStatement = "for" AssignmentExpression "to" Expression "by" Expression "while" '(' Expression ')' Statement "rof" ';'.
        private SyntaxTreeNode IterationStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("for");

            ExpectedToken(TokenTable.GetLexemeId("for"));
            node.AddChild(AssignmentExpression());
            ExpectedToken(TokenTable.GetLexemeId("to"));
            node.AddChild(Expression());
            ExpectedToken(TokenTable.GetLexemeId("by"));
            node.AddChild(Expression());
            ExpectedToken(TokenTable.GetLexemeId("while"));
            ExpectedToken(TokenTable.GetLexemeId("("));
            node.AddChild(Expression());
            ExpectedToken(TokenTable.GetLexemeId(")"));
            node.AddChild(Statement());
            ExpectedToken(TokenTable.GetLexemeId("rof"));
            ExpectedToken(TokenTable.GetLexemeId(";"));

            return node;
        }

        // CompoundStatement = '{' {Statement} '}'.
        private SyntaxTreeNode CompoundStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("statements");
            ExpectedToken(TokenTable.GetLexemeId("{"));
            while (GetTokenCode() != TokenTable.GetLexemeId("}"))
            {
                node.AddChild(Statement());
            }
            ExpectedToken(TokenTable.GetLexemeId("}"));
            return node;
        }

        // DeclarationStatement = ("int" | "float") Identifier ';'.
        private SyntaxTreeNode DeclarationStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("declaration");

            //"int" | "float"
            var token = ExpectedOneOfTokens("TypeSpecifier", TokenTable.GetLexemeId("int"), TokenTable.GetLexemeId("float"));
            node.AddChild(new SyntaxTreeNode(TokenTable.GetLexemeName(token.Code)));
            // identifier
            token = ExpectedToken(TokenTable.GetLexemeId("identifier"));
            node.AddChild(new SyntaxTreeNode("identifier", token.ForeignId));

            ExpectedToken(TokenTable.GetLexemeId(";"));

            return node;
        }

        // InputStatement = '$' Identifier ';'.
        private SyntaxTreeNode InputStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("input");

            ExpectedToken(TokenTable.GetLexemeId("$"));
            var token = ExpectedToken(TokenTable.GetLexemeId("identifier"));
            node.AddChild(new SyntaxTreeNode("identifier", token.ForeignId));
            ExpectedToken(TokenTable.GetLexemeId(";"));

            return node;
        }

        // OutputStatement = '@' Expression ';'.
        private SyntaxTreeNode OutputStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("output");

            ExpectedToken(TokenTable.GetLexemeId("@"));
            node.AddChild(Expression());
            ExpectedToken(TokenTable.GetLexemeId(";"));

            return node;
        }

        // AssignmentStatement = AssignmentExpression ';'.
        private SyntaxTreeNode AssignmentStatement()
        {
            SyntaxTreeNode node = AssignmentExpression();
            ExpectedToken(TokenTable.GetLexemeId(";"));
            return node;
        }
    }
}