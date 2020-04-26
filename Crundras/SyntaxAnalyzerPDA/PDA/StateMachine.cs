using LexicalAnalyzer;
using System;
using System.Collections.Generic;

namespace SyntaxAnalyzerPDA.PDA
{
    public class StateMachine
    {
        private readonly List<State> states = new List<State>();
        private readonly Stack<int> stack = new Stack<int>();

        private State StateById(int id)
        {
            return states.Find(s => s.Id == id);
        }

        public State CurrentState { get; private set; }

        public StateMachine()
        {
            State state;

            #region EXPRESSION
            state = new State(104, stack, true, false);
            states.Add(state);

            state = new State(103, stack)
                .ConfigureTransition(TokenTable.GetLexemeId(")"), StateById(102));
            states.Add(state);

            State state105 = new State(105, stack);
            states.Add(state105);

            state = new State(102, stack)
                .ConfigureTransition(15, StateById(105))
                .ConfigureTransition(16, StateById(105))
                .ConfigureTransition(17, StateById(105))
                .ConfigureTransition(18, StateById(105))
                .ConfigureTransition(19, StateById(105))
                .ConfigureTransition(20, StateById(105))
                .ConfigureTransition(21, StateById(105))
                .ConfigureTransition(22, StateById(105))
                .ConfigureTransition(23, StateById(105))
                .ConfigureTransition(24, StateById(105))
                .ConfigureTransition(25, StateById(105))
                .ConfigureTransition(26, StateById(105))
                .ConfigureOtherwiseTransition(StateById(104));
            states.Add(state);

            state105
                .ConfigureTransition(TokenTable.GetLexemeId("identifier"), StateById(102))
                .ConfigureTransition(TokenTable.GetLexemeId("int_literal"), StateById(102))
                .ConfigureTransition(TokenTable.GetLexemeId("float_literal"), StateById(102));

            state = new State(101, stack)
                .ConfigureTransition(TokenTable.GetLexemeId("("), StateById(100), null, 103)
                .ConfigureTransition(TokenTable.GetLexemeId("identifier"), StateById(102))
                .ConfigureTransition(TokenTable.GetLexemeId("int_literal"), StateById(102))
                .ConfigureTransition(TokenTable.GetLexemeId("float_literal"), StateById(102));
            states.Add(state);

            state = new State(100, stack)
                .ConfigureTransition(TokenTable.GetLexemeId("+"), StateById(101))
                .ConfigureTransition(TokenTable.GetLexemeId("-"), StateById(101))
                .ConfigureTransition(TokenTable.GetLexemeId("identifier"), StateById(102))
                .ConfigureTransition(TokenTable.GetLexemeId("int_literal"), StateById(102))
                .ConfigureTransition(TokenTable.GetLexemeId("float_literal"), StateById(102))
                .ConfigureSelfTransition(TokenTable.GetLexemeId("("), null, 103);
            states.Add(state);

            #endregion

            #region STATEMENT
            state = new State(3, stack, true);
            states.Add(state);

            state = new State(21, stack)
                .ConfigureTransition(TokenTable.GetLexemeId("}"), StateById(3));
            states.Add(state);

            state = new State(16, stack)
                .ConfigureTransition(TokenTable.GetLexemeId("("), StateById(100), null, 17);
            states.Add(state);

            state = new State(15, stack)
                .ConfigureTransition(TokenTable.GetLexemeId("while"), StateById(16));
            states.Add(state);

            state = new State(14, stack)
                .ConfigureTransition(TokenTable.GetLexemeId("by"), StateById(100), null, 15);
            states.Add(state);

            state = new State(13, stack)
                .ConfigureTransition(TokenTable.GetLexemeId("to"), StateById(100), null, 14);
            states.Add(state);

            state = new State(12, stack)
                .ConfigureTransition(TokenTable.GetLexemeId("="), StateById(100), null, 13);
            states.Add(state);

            state = new State(11, stack)
                .ConfigureTransition(TokenTable.GetLexemeId("identifier"), StateById(12));
            states.Add(state);

            state = new State(9, stack)
                .ConfigureTransition(TokenTable.GetLexemeId("("), StateById(100), null, 10);
            states.Add(state);

            state = new State(8, stack)
                .ConfigureTransition(TokenTable.GetLexemeId(";"), StateById(3));
            states.Add(state);
            state = new State(7, stack)
                .ConfigureTransition(TokenTable.GetLexemeId("="), StateById(100), null, 8);
            states.Add(state);

            state = new State(6, stack)
                .ConfigureTransition(TokenTable.GetLexemeId(";"), StateById(3));
            states.Add(state);

            state = new State(5, stack)
                .ConfigureTransition(TokenTable.GetLexemeId(";"), StateById(3));
            states.Add(state);

            state = new State(4, stack)
                .ConfigureTransition(TokenTable.GetLexemeId("identifier"), StateById(5));
            states.Add(state);

            state = new State(2, stack)
                .ConfigureTransition(TokenTable.GetLexemeId(";"), StateById(3));
            states.Add(state);

            state = new State(1, stack)
                .ConfigureTransition(TokenTable.GetLexemeId("identifier"), StateById(2));
            states.Add(state);

            state = new State(0, stack)
                    .ConfigureTransition(TokenTable.GetLexemeId("int"), StateById(1))
                    .ConfigureTransition(TokenTable.GetLexemeId("float"), StateById(1))
                    .ConfigureTransition(TokenTable.GetLexemeId("$"), StateById(4))
                    .ConfigureTransition(TokenTable.GetLexemeId("@"), StateById(100), null, 6)
                    .ConfigureTransition(TokenTable.GetLexemeId("identifier"), StateById(7))
                    .ConfigureTransition(TokenTable.GetLexemeId("if"), StateById(9))
                    .ConfigureTransition(TokenTable.GetLexemeId("for"), StateById(11))
                    .ConfigureSelfTransition(TokenTable.GetLexemeId("{"), null, 21);

            states.Add(state);

            state = new State(10, stack)
                .ConfigureTransition(TokenTable.GetLexemeId(")"), StateById(0), null, 3);
            states.Add(state);

            state = new State(19, stack)
                .ConfigureTransition(TokenTable.GetLexemeId(";"), StateById(3));
            states.Add(state);

            state = new State(18, stack)
                .ConfigureTransition(TokenTable.GetLexemeId("rof"), StateById(19));
            states.Add(state);

            state = new State(17, stack)
                .ConfigureTransition(TokenTable.GetLexemeId(")"), StateById(0), null, 18);
            states.Add(state);

            #endregion

            CurrentState = StateById(0);
        }

        public void NextState(uint tokenType)
        {
            if (!CurrentState.IsFinal)
            {
                var previousState = CurrentState;
                CurrentState = CurrentState.Transit(tokenType);
                if (CurrentState == null)
                {
                    throw new Exception($"lexeme: {TokenTable.GetLexemeName(tokenType)}. state: {previousState.Id}");
                }

                return;
            }

            if (stack.Count == 0)
            {
                CurrentState = StateById(0);
            }
            else
            {
                int nextStateId = stack.Pop();
                CurrentState = StateById(nextStateId);
            }

            NextState(tokenType);
        }
    }
}
