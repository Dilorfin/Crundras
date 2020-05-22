using Crundras.Common;
using Crundras.Common.Tables;
using SyntaxAnalyzerPDA.PDA;
using System;
using System.Collections.Generic;

namespace SyntaxAnalyzerPDA
{
    public class SyntaxAnalyzer
    {
        private readonly StateMachine stateMachine;
        private readonly TablesCollection tables;

        private LinkedListNode<Token> tokenListNode;

        public SyntaxTreeNode RootTreeNode { get; }
        public SyntaxTreeNode CurrentTreeNode { get; private set; }

        public SyntaxAnalyzer(TablesCollection tables)
        {
            this.tables = tables;
            tokenListNode = tables.TokenTable.First;

            RootTreeNode = new SyntaxTreeNode(uint.MaxValue);
            CurrentTreeNode = RootTreeNode;

            stateMachine = new StateMachine();
        }

        public static SyntaxTreeNode Analyze(TablesCollection tables)
        {
            var analyzer = new SyntaxAnalyzer(tables);
            analyzer.Analyze();
            return analyzer.RootTreeNode;
        }

        private void Analyze()
        {
            while (tokenListNode != null)
            {
                AnalyzeToken();
            }

        }

        public void AnalyzeToken()
        {
            do
            {
                stateMachine.NextState(tokenListNode.Value.Code);

                if (stateMachine.CurrentState.IsError)
                {
                    var message = (stateMachine.CurrentState as ErrorState)?.Message;
                    throw new Exception(message);
                }

                // delicate balancing bugs
                if ((stateMachine.CurrentState.Id == 11
                    && tokenListNode.Next != null
                    && tokenListNode.Next.Value.Code == LexemesTable.GetLexemeId(":"))
                    || stateMachine.CurrentState.Id == 8)
                {
                    var value = tokenListNode.Value;

                    value.Code = LexemesTable.GetLexemeId("label");

                    var identifier = tables.IdentifiersTable[value.ForeignId.Value];
                    value.ForeignId = tables.LabelsTable.GetId(identifier.Name);

                    tokenListNode.Value = value;
                }

                var child = new SyntaxTreeNode(tokenListNode.Value);

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


            } while (!stateMachine.CurrentState.TakeToken);

            tokenListNode = tokenListNode.Next;
        }
    }
}