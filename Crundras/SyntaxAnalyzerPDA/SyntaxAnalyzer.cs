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
                stateMachine.NextState(tokenListNode.Value.Code);

                if (stateMachine.CurrentState.IsError)
                {
                    var message = (stateMachine.CurrentState as ErrorState)?.Message;
                    throw new Exception(message);
                }

                if ((stateMachine.CurrentState.Id == 11
                     && tokenListNode?.Next?.Value.Code == LexemesTable.GetLexemeId(":")) 
                    || stateMachine.CurrentState.Id == 8)
                {
                    var value = tokenListNode.Value;

                    value.Code = LexemesTable.GetLexemeId("label");

                    var identifier = tables.IdentifiersTable[value.ForeignId.Value];
                    value.ForeignId = tables.LabelsTable.GetId(identifier.Name);

                    tokenListNode.Value = value;
                }

                SyntaxTreeBuilder();

                if (stateMachine.CurrentState.TakeToken)
                {
                    tokenListNode = tokenListNode.Next;
                }
            }
        }
        
        public void SyntaxTreeBuilder()
        {
            if (stateMachine.CurrentState.Id == 105)
            {
                CurrentTreeNode = CurrentTreeNode.Parent;
                if (CurrentTreeNode.LexemeCode != LexemesTable.GetLexemeId("if"))
                {
                    CurrentTreeNode = CurrentTreeNode.Parent;
                }
            }

            if (CurrentTreeNode.LexemeCode == LexemesTable.GetLexemeId("if")
                && CurrentTreeNode.Children?.Count == 2)
            {
                CurrentTreeNode = CurrentTreeNode.Parent;
            }

            if (!stateMachine.CurrentState.TakeToken)
            {
                return;
            }

            if (tokenListNode.Value.Code == LexemesTable.GetLexemeId(":"))
            {
                CurrentTreeNode.AddChild(new SyntaxTreeNode(tokenListNode.Previous.Value));
            }
            else if (tokenListNode.Value.Code == LexemesTable.GetLexemeId("="))
            {
                CurrentTreeNode.AddChild(new SyntaxTreeNode(tokenListNode.Value));
                CurrentTreeNode = CurrentTreeNode.Children[^1];
                CurrentTreeNode.AddChild(new SyntaxTreeNode(tokenListNode.Previous.Value));
            }
            else if (new List<uint> {
                    LexemesTable.GetLexemeId("if"),
                    LexemesTable.GetLexemeId("{"),
                    LexemesTable.GetLexemeId("@"),
                    LexemesTable.GetLexemeId("for"),
                    LexemesTable.GetLexemeId("to"),
                    LexemesTable.GetLexemeId("by"),
                    LexemesTable.GetLexemeId("while")
                }.Contains(tokenListNode.Value.Code))
            {
                CurrentTreeNode.AddChild(new SyntaxTreeNode(tokenListNode.Value));
                CurrentTreeNode = CurrentTreeNode.Children[^1];
            }
            else if (tokenListNode.Value.Code == LexemesTable.GetLexemeId("}"))
            {
                CurrentTreeNode = CurrentTreeNode.Parent;
            }
            else if (tokenListNode.Value.Code == LexemesTable.GetLexemeId("rof"))
            {
                CurrentTreeNode = CurrentTreeNode.Parent;
            }

            if (new List<int>{ 1, 4, 7 }.Contains(stateMachine.CurrentState.Id))
            {
                CurrentTreeNode.AddChild(new SyntaxTreeNode(tokenListNode.Value));
                CurrentTreeNode = CurrentTreeNode.Children[^1];
            }
            else if (new List<int>{ 2, 5, 8 }.Contains(stateMachine.CurrentState.Id))
            {
                CurrentTreeNode.AddChild(new SyntaxTreeNode(tokenListNode.Value));
                CurrentTreeNode = CurrentTreeNode.Parent;
            }
            else if (stateMachine.CurrentState.Id == 100)
            {
                CurrentTreeNode.AddChild(new SyntaxTreeNode(100));
                CurrentTreeNode = CurrentTreeNode.Children[^1];
            }
            else if (stateMachine.CurrentState.Id > 100)
            {
                CurrentTreeNode.AddChild(new SyntaxTreeNode(tokenListNode.Value));
            }
        }
    }
}