using System;
using System.Collections.Generic;
using LexicalAnalyzer;

namespace Recursive_descent_parser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    public class SyntaxTreeNode
    {
        private List<SyntaxTreeNode> children = null;

        public string Name { get; }
        public uint Id { get; }

        public SyntaxTreeNode(string name, uint id = 0)
        {
            Name = name;
            Id = id;
        }

        public void AddChild(SyntaxTreeNode node)
        {
            if (children == null)
            {
                children = new List<SyntaxTreeNode>();
            }

            children.Add(node);
        }

        public List<SyntaxTreeNode> GetChildren()
        {
            return children;
        }
    }
    
    public class RecursiveDescentParser
    {
        private LinkedListNode<TokenTable.Token> tokenListNode;
        
        SyntaxTreeNode _Program()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("statements");
            
            while (TransitToNextToken())
            {
                node.AddChild(Statement());
            }

            return node;
        }

        // Statement = InputStatement | OutputStatement | CompoundStatement | ExpressionStatement | SelectionStatement | IterationStatement | DeclarationStatement.
        SyntaxTreeNode Statement()
        {
            SyntaxTreeNode node = null;

            switch (GetTokenCode())
            {
                case 12: // '$'
                    node = InputStatement();
                    break;
                case 13: // '@'
                    node = OutputStatement();
                    break;
                case 29: // '{'
                    node = CompoundStatement();
                    break;
                case 6: // 'if'
                    node = SelectionStatement();
                    break;
                case 7: // 'for'
                    node = IterationStatement();
                    break;
                case 4: // 'int'
                case 5: // 'float'
                    node = DeclarationStatement();
                    break;
                default:
                    node = ExpressionStatement();
                    break;
            }

            return node;
        }

        uint GetTokenCode()
        {
            return tokenListNode.Value.code;
        }
        uint GetNextTokenCode()
        {
            return tokenListNode.Next.Value.code;
        }

        bool TransitToNextToken()
        {
            tokenListNode = tokenListNode.Next;
            return tokenListNode != null;
        }

        void ExpectedToken(uint tokenCode)
        {
            if (GetTokenCode() == tokenCode)
                TransitToNextToken();
            else throw new Exception("");
        }

        // ExpressionStatement = Expression ';'.
        SyntaxTreeNode ExpressionStatement()
        {
            Expression();
            return null;
        }
        // SelectionStatement = "if" '(' Expression ')' Statement.
        SyntaxTreeNode SelectionStatement()
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
        SyntaxTreeNode IterationStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("for");

            ExpectedToken(7); // 'for'
            node.AddChild(Expression()); // TODO: Expression => AssignmentExpression
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
        SyntaxTreeNode CompoundStatement()
        {
            SyntaxTreeNode node = null;
            ExpectedToken(29); // '{'
            node = Statement();
            ExpectedToken(30); // '}'
            return node;
        }

        // DeclarationStatement = ("int" | "float") Identifier ';'.
        SyntaxTreeNode DeclarationStatement(){return null;}

        // InputStatement = '$' Identifier ';'.
        SyntaxTreeNode InputStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("input");

            ExpectedToken(12); // '$'
            
            node.AddChild(new SyntaxTreeNode("identifier", tokenListNode.Value.id));
            ExpectedToken(1); // identifier
            ExpectedToken(31); // ';'

            return node;
        }
        // OutputStatement = '@' Expression ';'.
        SyntaxTreeNode OutputStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("input");

            ExpectedToken(13); // '@'
            node.AddChild(Expression());
            ExpectedToken(31); // ';'

            return node;
        }

        SyntaxTreeNode Expression()
        {
            return null;
        }
    }
}
