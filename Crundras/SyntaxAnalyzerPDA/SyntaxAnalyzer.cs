using Crundras.Common;
using SyntaxAnalyzerPDA.PDA;
using System;
using System.Collections.Generic;

namespace SyntaxAnalyzerPDA
{
    public class SyntaxAnalyzer
    {
        private readonly StateMachine stateMachine;

        public SyntaxTreeRootNode RootTreeNode { get; }
        public SyntaxTreeNode CurrentTreeNode { get; private set; }

        public SyntaxAnalyzer()
        {
            RootTreeNode = new SyntaxTreeRootNode();
            CurrentTreeNode = RootTreeNode;
            
            stateMachine = new StateMachine();
        }

        public static SyntaxTreeRootNode Analyze(TokenTable tokenTable)
        {
            var analyzer = new SyntaxAnalyzer();

            LinkedListNode<Token> tokenListNode = tokenTable.TokensList.First;
            while (tokenListNode != null)
            {
                analyzer.Analyze(tokenListNode.Value);
                tokenListNode = tokenListNode.Next;
            }

            return analyzer.RootTreeNode;
        }

        public void Analyze(Token token)
        {
            do
            {
                stateMachine.NextState(token.Code);

                if (stateMachine.CurrentState.IsError)
                {
                    var message = (stateMachine.CurrentState as ErrorState)?.Message;
                    throw new Exception(message);
                }

                var child = new SyntaxTreeNode(token);

                if (stateMachine.CurrentState.TakeToken)
                {
                    CurrentTreeNode.AddChild(child);

                    if (stateMachine.CurrentState.IsLevelStart)
                    {
                        CurrentTreeNode = child;
                    }
                }

                if (stateMachine.CurrentState.IsFinal
                    && CurrentTreeNode.Parent != null)
                {
                    CurrentTreeNode = CurrentTreeNode.Parent;
                }
            } 
            while (!stateMachine.CurrentState.TakeToken);
        }
    }
}