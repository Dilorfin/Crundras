using LexicalAnalyzer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Recursive_descent_parser
{
    public partial class RecursiveDescentParser
    {
        private LinkedListNode<Token> tokenListNode;

        public RecursiveDescentParser(TokenTable tokenTable)
        {
            tokenListNode = tokenTable.TokensList.First;
        }

        public SyntaxTreeNode Analyze()
        {
            return _Program();
        }

        private SyntaxTreeNode _Program()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("statements");

            while (tokenListNode != null)
            {
                node.AddChild(Statement());
            }

            return node;
        }

        private uint GetTokenCode()
        {
            return tokenListNode.Value.Code;
        }

        private void CheckUnexpectedEnd()
        {
            if (tokenListNode == null)
            {
                throw new Exception("Unexpected end of the program.");
            }
        }

        private void TransitToNextToken()
        {
            CheckUnexpectedEnd();

            tokenListNode = tokenListNode.Next;
        }

        private Token ExpectedOneOfTokens(string expected, params uint[] tokenCodes)
        {
            CheckUnexpectedEnd();

            if (tokenCodes.All(tokenCode => GetTokenCode() != tokenCode))
            {
                throw new Exception($"Expected {expected} but found \"{TokenTable.GetLexemeName(GetTokenCode())}\" in line {tokenListNode.Value.Line}.");
            }

            var token = tokenListNode.Value;
            TransitToNextToken();
            return token;
        }

        private Token ExpectedToken(uint tokenCode)
        {
            CheckUnexpectedEnd();

            if (GetTokenCode() != tokenCode)
            {
                throw new Exception($"Expected \"{TokenTable.GetLexemeName(tokenCode)}\" " +
                                    $"but found \"{TokenTable.GetLexemeName(GetTokenCode())}\" " +
                                    $"in line {tokenListNode.Value.Line}.");
            }

            var token = tokenListNode.Value;
            TransitToNextToken();
            return token;
        }
    }
}