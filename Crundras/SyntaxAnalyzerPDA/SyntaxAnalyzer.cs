using LexicalAnalyzer;
using SyntaxAnalyzerPDA.PDA;
using System;
using System.Collections.Generic;

namespace SyntaxAnalyzerPDA
{
    public class SyntaxAnalyzer
    {
        private LinkedListNode<TokenTable.Token> tokenListNode;
        private SyntaxTreeNode syntaxTree = new SyntaxTreeNode("statements");

        public SyntaxAnalyzer(TokenTable tokenTable)
        {
            this.tokenListNode = tokenTable.TokensList.First;
        }

        public SyntaxTreeNode Analyze()
        {
            StateMachine stateMachine = new StateMachine();

            var syntaxTreeNode = syntaxTree;

            while (tokenListNode != null)
            {
                var token = tokenListNode.Value;
                stateMachine.NextState(token.Code);

                if (stateMachine.CurrentState.IsError)
                {
                    // TODO: make error message more informative
                    var message = (stateMachine.CurrentState as ErrorState)?.Message;
                    throw new Exception(message);
                }

                /*if (stateMachine.CurrentState.IsContinue)
                {

                }*/

                if (stateMachine.CurrentState.IsStart)
                {
                    // TODO: make node name selection
                    var node = new SyntaxTreeNode("parent");
                    syntaxTreeNode.AddChild(node);
                    syntaxTreeNode = node;
                }
                else if (stateMachine.CurrentState.IsFinal && syntaxTreeNode.Parent != null)
                {
                    syntaxTreeNode = syntaxTreeNode.Parent;
                    continue;
                }

                uint id = tokenListNode.Value.ForeignId;
                string name = tokenListNode.Value.Lexeme;
                syntaxTreeNode.AddChild(new SyntaxTreeNode(name, id));

                tokenListNode = tokenListNode.Next;
            }

            if (!stateMachine.IsFinished)
            {
                throw new Exception("Unexpected end of program.");
            }

            return syntaxTree;
        }
    }
}