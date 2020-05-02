using Crundras.Common;

namespace RecursiveDescentParser
{
    public partial class RecursiveDescentParser
    {
        // Statement = InputStatement | OutputStatement | CompoundStatement | ExpressionStatement | SelectionStatement | IterationStatement | DeclarationStatement | AssignmentStatement.
        private SyntaxTreeNode Statement()
        {
            var node = TokenCode switch
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
                //_ => ExpressionStatement()
            };

            return node;
        }

        // ExpressionStatement = Expression ';'.
        /*private SyntaxTreeNode ExpressionStatement()
        {
            SyntaxTreeNode node = Expression();
            ExpectedToken(TokenTable.GetLexemeId(";"));
            return node;
        }*/

        // SelectionStatement = "if" '(' Expression ')' Statement.
        private SyntaxTreeNode SelectionStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode(TokenCode);
            ExpectedToken(TokenTable.GetLexemeId("if"));

            ExpectedToken(TokenTable.GetLexemeId("("));
            node.AddChildren(Expression());
            ExpectedToken(TokenTable.GetLexemeId(")"));

            node.AddChild(Statement());

            return node;
        }

        // IterationStatement = "for" AssignmentExpression "to" Expression "by" Expression "while" '(' Expression ')' Statement "rof" ';'.
        private SyntaxTreeNode IterationStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode(TokenCode);
            ExpectedToken(TokenTable.GetLexemeId("for"));
            
            node.AddChild(AssignmentExpression());
            
            node.AddChild(new SyntaxTreeNode(TokenCode));
            ExpectedToken(TokenTable.GetLexemeId("to"));
            node.Children[^1].AddChildren(Expression());
            
            node.AddChild(new SyntaxTreeNode(TokenCode));
            ExpectedToken(TokenTable.GetLexemeId("by"));
            node.Children[^1].AddChildren(Expression());

            node.AddChild(new SyntaxTreeNode(TokenCode));
            ExpectedToken(TokenTable.GetLexemeId("while"));

            ExpectedToken(TokenTable.GetLexemeId("("));
            node.Children[^1].AddChildren(Expression());
            ExpectedToken(TokenTable.GetLexemeId(")"));

            node.AddChild(Statement());

            ExpectedToken(TokenTable.GetLexemeId("rof"));
            ExpectedToken(TokenTable.GetLexemeId(";"));

            return node;
        }

        // CompoundStatement = '{' {Statement} '}'.
        private SyntaxTreeNode CompoundStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode(TokenCode);
            ExpectedToken(TokenTable.GetLexemeId("{"));
            while (TokenCode != TokenTable.GetLexemeId("}"))
            {
                node.AddChild(Statement());
            }
            ExpectedToken(TokenTable.GetLexemeId("}"));
            return node;
        }

        // DeclarationStatement = ("int" | "float") Identifier ';'.
        private SyntaxTreeNode DeclarationStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode(TokenCode);
            
            //"int" | "float"
            ExpectedOneOfTokens("TypeSpecifier", TokenTable.GetLexemeId("int"), TokenTable.GetLexemeId("float"));
            
            // identifier
            var token = ExpectedToken(TokenTable.GetLexemeId("identifier"));
            node.AddChild(new SyntaxTreeNode(token));

            ExpectedToken(TokenTable.GetLexemeId(";"));

            return node;
        }

        // InputStatement = '$' Identifier ';'.
        private SyntaxTreeNode InputStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode(TokenCode);
            
            ExpectedToken(TokenTable.GetLexemeId("$"));
            var token = ExpectedToken(TokenTable.GetLexemeId("identifier"));
            node.AddChild(new SyntaxTreeNode(token));
            ExpectedToken(TokenTable.GetLexemeId(";"));

            return node;
        }

        // OutputStatement = '@' Expression ';'.
        private SyntaxTreeNode OutputStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode(TokenCode);

            ExpectedToken(TokenTable.GetLexemeId("@"));
            node.AddChildren(Expression());
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