using Crundras.Common;
using System;
using System.Collections.Generic;

namespace SyntaxAnalyzerPDA.PDA
{
    public class StateMachine
    {
        private readonly Dictionary<int, State> states = new Dictionary<int, State>();
        private readonly Stack<int> stack = new Stack<int>();

        public State CurrentState { get; private set; }

        public StateMachine()
        {
            State state;

            #region EXPRESSION
            state = new State(104, stack, true, false);
            states.Add(state.Id, state);

            state = new State(103, stack)
                .ConfigureTransition(TokenTable.GetLexemeId(")"), 102);
            states.Add(state.Id, state);

            state = new State(105, stack)
                .ConfigureTransition(TokenTable.GetLexemeId("+"), 101)
                .ConfigureTransition(TokenTable.GetLexemeId("-"), 101)
                .ConfigureTransition(TokenTable.GetLexemeId("identifier"), 102)
                .ConfigureTransition(TokenTable.GetLexemeId("int_literal"), 102)
                .ConfigureTransition(TokenTable.GetLexemeId("float_literal"), 102)
                .ConfigureTransition(TokenTable.GetLexemeId("("), 100, null, 103);

            states.Add(state.Id, state);

            state = new State(102, stack)
                .ConfigureTransition(15, 105)
                .ConfigureTransition(16, 105)
                .ConfigureTransition(17, 105)
                .ConfigureTransition(18, 105)
                .ConfigureTransition(19, 105)
                .ConfigureTransition(20, 105)
                .ConfigureTransition(21, 105)
                .ConfigureTransition(22, 105)
                .ConfigureTransition(23, 105)
                .ConfigureTransition(24, 105)
                .ConfigureTransition(25, 105)
                .ConfigureTransition(26, 105)
                .ConfigureOtherwiseTransition(104);
            states.Add(state.Id, state);

            state = new State(101, stack)
                .ConfigureTransition(TokenTable.GetLexemeId("("), 100, null, 103)
                .ConfigureTransition(TokenTable.GetLexemeId("identifier"), 102)
                .ConfigureTransition(TokenTable.GetLexemeId("int_literal"), 102)
                .ConfigureTransition(TokenTable.GetLexemeId("float_literal"), 102);
            states.Add(state.Id, state);

            state = new State(100, stack)
                .ConfigureTransition(TokenTable.GetLexemeId("+"), 101)
                .ConfigureTransition(TokenTable.GetLexemeId("-"), 101)
                .ConfigureTransition(TokenTable.GetLexemeId("identifier"), 102)
                .ConfigureTransition(TokenTable.GetLexemeId("int_literal"), 102)
                .ConfigureTransition(TokenTable.GetLexemeId("float_literal"), 102)
                .ConfigureSelfTransition(TokenTable.GetLexemeId("("), null, 103);
            states.Add(state.Id, state);

            #endregion

            #region STATEMENT
            state = new State(3, stack, true);
            states.Add(state.Id, state);

            state = new State(21, stack)
                .ConfigureTransition(TokenTable.GetLexemeId("}"), 3);
            states.Add(state.Id, state);

            state = new State(16, stack)
                .ConfigureTransition(TokenTable.GetLexemeId("("), 100, null, 17);
            states.Add(state.Id, state);

            state = new State(15, stack)
                .ConfigureTransition(TokenTable.GetLexemeId("while"), 16);
            states.Add(state.Id, state);

            state = new State(14, stack)
                .ConfigureTransition(TokenTable.GetLexemeId("by"), 100, null, 15);
            states.Add(state.Id, state);

            state = new State(13, stack)
                .ConfigureTransition(TokenTable.GetLexemeId("to"), 100, null, 14);
            states.Add(state.Id, state);

            state = new State(12, stack)
                .ConfigureTransition(TokenTable.GetLexemeId("="), 100, null, 13);
            states.Add(state.Id, state);

            state = new State(11, stack)
                .ConfigureTransition(TokenTable.GetLexemeId("identifier"), 12);
            states.Add(state.Id, state);

            state = new State(9, stack)
                .ConfigureTransition(TokenTable.GetLexemeId("("), 100, null, 10);
            states.Add(state.Id, state);

            state = new State(8, stack)
                .ConfigureTransition(TokenTable.GetLexemeId(";"), 3);
            states.Add(state.Id, state);
            state = new State(7, stack)
                .ConfigureTransition(TokenTable.GetLexemeId("="), 100, null, 8);
            states.Add(state.Id, state);

            state = new State(6, stack)
                .ConfigureTransition(TokenTable.GetLexemeId(";"), 3);
            states.Add(state.Id, state);

            state = new State(5, stack)
                .ConfigureTransition(TokenTable.GetLexemeId(";"), 3);
            states.Add(state.Id, state);

            state = new State(4, stack)
                .ConfigureTransition(TokenTable.GetLexemeId("identifier"), 5);
            states.Add(state.Id, state);

            state = new State(2, stack)
                .ConfigureTransition(TokenTable.GetLexemeId(";"), 3);
            states.Add(state.Id, state);

            state = new State(1, stack)
                .ConfigureTransition(TokenTable.GetLexemeId("identifier"), 2);
            states.Add(state.Id, state);

            state = new State(0, stack)
                    .ConfigureTransition(TokenTable.GetLexemeId("int"), 1)
                    .ConfigureTransition(TokenTable.GetLexemeId("float"), 1)
                    .ConfigureTransition(TokenTable.GetLexemeId("$"), 4)
                    .ConfigureTransition(TokenTable.GetLexemeId("@"), 100, null, 6)
                    .ConfigureTransition(TokenTable.GetLexemeId("identifier"), 7)
                    .ConfigureTransition(TokenTable.GetLexemeId("if"), 9)
                    .ConfigureTransition(TokenTable.GetLexemeId("for"), 11)
                    .ConfigureSelfTransition(TokenTable.GetLexemeId("{"), null, 21);

            states.Add(state.Id, state);

            state = new State(10, stack)
                .ConfigureTransition(TokenTable.GetLexemeId(")"), 0, null, 3);
            states.Add(state.Id, state);

            state = new State(19, stack)
                .ConfigureTransition(TokenTable.GetLexemeId(";"), 3);
            states.Add(state.Id, state);

            state = new State(18, stack)
                .ConfigureTransition(TokenTable.GetLexemeId("rof"), 19);
            states.Add(state.Id, state);

            state = new State(17, stack)
                .ConfigureTransition(TokenTable.GetLexemeId(")"), 0, null, 18);
            states.Add(state.Id, state);

            #endregion

            CurrentState = states[0];
        }

        public void NextState(uint tokenType)
        {
            if (!CurrentState.IsFinal)
            {
                var previousState = CurrentState;
                CurrentState = states[CurrentState.Transit(tokenType)];
                if (CurrentState == null)
                {
                    throw new Exception($"lexeme: {TokenTable.GetLexemeName(tokenType)}. state: {previousState.Id}");
                }

                return;
            }

            if (stack.Count == 0)
            {
                CurrentState = states[0];
            }
            else
            {
                int nextStateId = stack.Pop();
                CurrentState = states[nextStateId];
            }

            NextState(tokenType);
        }
    }
}
