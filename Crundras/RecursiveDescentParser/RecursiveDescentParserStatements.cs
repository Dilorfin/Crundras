using Crundras.Common;
using Crundras.Common.Tables;

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
            ExpectedToken(LexemesTable.GetLexemeId(";"));
            return node;
        }*/

        // SelectionStatement = "if" '(' Expression ')' Statement.
        private SyntaxTreeNode SelectionStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode(TokenCode);
            ExpectedToken(LexemesTable.GetLexemeId("if"));

            ExpectedToken(LexemesTable.GetLexemeId("("));
            node.AddChildren(Expression());
            ExpectedToken(LexemesTable.GetLexemeId(")"));

            node.AddChild(Statement());

            return node;
        }

        // IterationStatement = "for" AssignmentExpression "to" Expression "by" Expression "while" '(' Expression ')' Statement "rof" ';'.
        private SyntaxTreeNode IterationStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode(TokenCode);
            ExpectedToken(LexemesTable.GetLexemeId("for"));

            node.AddChild(AssignmentExpression());

            node.AddChild(new SyntaxTreeNode(TokenCode));
            ExpectedToken(LexemesTable.GetLexemeId("to"));
            node.Children[^1].AddChildren(Expression());

            node.AddChild(new SyntaxTreeNode(TokenCode));
            ExpectedToken(LexemesTable.GetLexemeId("by"));
            node.Children[^1].AddChildren(Expression());

            node.AddChild(new SyntaxTreeNode(TokenCode));
            ExpectedToken(LexemesTable.GetLexemeId("while"));

            ExpectedToken(LexemesTable.GetLexemeId("("));
            node.Children[^1].AddChildren(Expression());
            ExpectedToken(LexemesTable.GetLexemeId(")"));

            node.AddChild(Statement());

            ExpectedToken(LexemesTable.GetLexemeId("rof"));
            ExpectedToken(LexemesTable.GetLexemeId(";"));

            return node;
        }

        // CompoundStatement = '{' {Statement} '}'.
        private SyntaxTreeNode CompoundStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode(TokenCode);
            ExpectedToken(LexemesTable.GetLexemeId("{"));
            while (TokenCode != LexemesTable.GetLexemeId("}"))
            {
                node.AddChild(Statement());
            }
            ExpectedToken(LexemesTable.GetLexemeId("}"));
            return node;
        }

        // DeclarationStatement = ("int" | "float") Identifier ';'.
        private SyntaxTreeNode DeclarationStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode(TokenCode);

            //"int" | "float"
            ExpectedOneOfTokens("TypeSpecifier", LexemesTable.GetLexemeId("int"), LexemesTable.GetLexemeId("float"));

            // identifier
            var token = ExpectedToken(LexemesTable.GetLexemeId("identifier"));
            node.AddChild(new SyntaxTreeNode(token));

            ExpectedToken(LexemesTable.GetLexemeId(";"));

            return node;
        }

        // InputStatement = '$' Identifier ';'.
        private SyntaxTreeNode InputStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode(TokenCode);

            ExpectedToken(LexemesTable.GetLexemeId("$"));
            var token = ExpectedToken(LexemesTable.GetLexemeId("identifier"));
            node.AddChild(new SyntaxTreeNode(token));
            ExpectedToken(LexemesTable.GetLexemeId(";"));

            return node;
        }

        // OutputStatement = '@' Expression ';'.
        private SyntaxTreeNode OutputStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode(TokenCode);

            ExpectedToken(LexemesTable.GetLexemeId("@"));
            node.AddChildren(Expression());
            ExpectedToken(LexemesTable.GetLexemeId(";"));

            return node;
        }

        // AssignmentStatement = AssignmentExpression ';'.
        private SyntaxTreeNode AssignmentStatement()
        {
            SyntaxTreeNode node = AssignmentExpression();
            ExpectedToken(LexemesTable.GetLexemeId(";"));
            return node;
        }
    }
}