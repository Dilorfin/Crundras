using Crundras.Common;
using SyntaxAnalyzerPDA.PDA;
using System;
using System.Collections.Generic;

namespace SyntaxAnalyzerPDA
{
    public class SyntaxAnalyzer
    {
        private LinkedListNode<Token> tokenListNode;
        private readonly SyntaxTreeRootNode syntaxTree = new SyntaxTreeRootNode();

        public SyntaxAnalyzer(TokenTable tokenTable)
        {
            this.tokenListNode = tokenTable.TokensList.First;
        }

        public SyntaxTreeRootNode Analyze()
        {
            var stateMachine = new StateMachine();

            SyntaxTreeNode syntaxTreeNode = syntaxTree;

            while (tokenListNode != null)
            {
                var token = tokenListNode.Value;
                stateMachine.NextState(token.Code);

                if (stateMachine.CurrentState.IsError)
                {
                    var message = (stateMachine.CurrentState as ErrorState)?.Message;
                    throw new Exception(message);
                }

                var child = new SyntaxTreeNode(token);

                if (stateMachine.CurrentState.TakeToken)
                {
                    syntaxTreeNode.AddChild(child);

                    if (stateMachine.CurrentState.IsLevelStart)
                    {
                        syntaxTreeNode = child;
                    }

                    tokenListNode = tokenListNode.Next;
                }


                if (stateMachine.CurrentState.IsFinal
                         && syntaxTreeNode.Parent != null)
                {
                    syntaxTreeNode = syntaxTreeNode.Parent;
                }
            }

            return syntaxTree;
        }
    }
}