using Crundras.Common.Tables;
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
            state = new State(105, stack, true, false);
            states.Add(state.Id, state);

            state = new State(103, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("("), 106, null, ')')
                .ConfigureTransition(LexemesTable.GetLexemeId("+"), 102)
                .ConfigureTransition(LexemesTable.GetLexemeId("-"), 102)
                .ConfigureTransition(LexemesTable.GetLexemeId("identifier"), 104)
                .ConfigureTransition(LexemesTable.GetLexemeId("int_literal"), 104)
                .ConfigureTransition(LexemesTable.GetLexemeId("float_literal"), 104);

            states.Add(state.Id, state);

            state = new State(104, stack)
                .ConfigureSelfTransition(LexemesTable.GetLexemeId(")"), ')')
                .ConfigureTransition(15, 103)
                .ConfigureTransition(16, 103)
                .ConfigureTransition(17, 103)
                .ConfigureTransition(18, 103)
                .ConfigureTransition(19, 103)
                .ConfigureTransition(20, 103)
                .ConfigureTransition(21, 103)
                .ConfigureTransition(22, 103)
                .ConfigureTransition(23, 103)
                .ConfigureTransition(24, 103)
                .ConfigureTransition(25, 103)
                .ConfigureTransition(26, 103)
                .ConfigureOtherwiseTransition(105);
            states.Add(state.Id, state);

            state = new State(102, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("("), 106, null, ')')
                .ConfigureTransition(LexemesTable.GetLexemeId("identifier"), 104)
                .ConfigureTransition(LexemesTable.GetLexemeId("int_literal"), 104)
                .ConfigureTransition(LexemesTable.GetLexemeId("float_literal"), 104);
            states.Add(state.Id, state);

            state = new State(106, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("+"), 102)
                .ConfigureTransition(LexemesTable.GetLexemeId("-"), 102)
                .ConfigureTransition(LexemesTable.GetLexemeId("identifier"), 101)
                .ConfigureTransition(LexemesTable.GetLexemeId("int_literal"), 101)
                .ConfigureTransition(LexemesTable.GetLexemeId("float_literal"), 101)
                .ConfigureSelfTransition(LexemesTable.GetLexemeId("("), null, ')');
            states.Add(state.Id, state);

            state = new State(101, stack, false, false)
                .ConfigureTransition(LexemesTable.GetLexemeId("+"), 102)
                .ConfigureTransition(LexemesTable.GetLexemeId("-"), 102)
                .ConfigureTransition(LexemesTable.GetLexemeId("identifier"), 104)
                .ConfigureTransition(LexemesTable.GetLexemeId("int_literal"), 104)
                .ConfigureTransition(LexemesTable.GetLexemeId("float_literal"), 104)
                .ConfigureTransition(LexemesTable.GetLexemeId("("), 106, null, ')');

            states.Add(state.Id, state);

            states[100] = new State(100, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("+"), 101)
                .ConfigureTransition(LexemesTable.GetLexemeId("-"), 101)
                .ConfigureTransition(LexemesTable.GetLexemeId("identifier"), 101)
                .ConfigureTransition(LexemesTable.GetLexemeId("int_literal"), 101)
                .ConfigureTransition(LexemesTable.GetLexemeId("float_literal"), 101)
                .ConfigureTransition(LexemesTable.GetLexemeId("("), 101);

            #endregion

            #region STATEMENT
            state = new State(3, stack, true);
            states.Add(state.Id, state);

            state = new State(21, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("}"), 3);
            states.Add(state.Id, state);

            state = new State(16, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("("), 100, null, 17);
            states.Add(state.Id, state);

            state = new State(15, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("while"), 16);
            states.Add(state.Id, state);

            state = new State(14, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("by"), 100, null, 15);
            states.Add(state.Id, state);

            state = new State(13, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("to"), 100, null, 14);
            states.Add(state.Id, state);

            state = new State(12, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("="), 100, null, 13);
            states.Add(state.Id, state);

            state = new State(11, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("identifier"), 12);
            states.Add(state.Id, state);

            state = new State(9, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("("), 100, null, 10);
            states.Add(state.Id, state);

            state = new State(8, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId(";"), 3);
            states.Add(state.Id, state);
            state = new State(7, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("="), 100, null, 8);
            states.Add(state.Id, state);

            state = new State(6, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId(";"), 3);
            states.Add(state.Id, state);

            state = new State(5, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId(";"), 3);
            states.Add(state.Id, state);

            state = new State(4, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("identifier"), 5);
            states.Add(state.Id, state);

            state = new State(2, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId(";"), 3);
            states.Add(state.Id, state);

            state = new State(1, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("identifier"), 2);
            states.Add(state.Id, state);

            state = new State(0, stack)
                    .ConfigureTransition(LexemesTable.GetLexemeId("int"), 1)
                    .ConfigureTransition(LexemesTable.GetLexemeId("float"), 1)
                    .ConfigureTransition(LexemesTable.GetLexemeId("$"), 4)
                    .ConfigureTransition(LexemesTable.GetLexemeId("@"), 100, null, 6)
                    .ConfigureTransition(LexemesTable.GetLexemeId("identifier"), 7)
                    .ConfigureTransition(LexemesTable.GetLexemeId("if"), 9)
                    .ConfigureTransition(LexemesTable.GetLexemeId("for"), 11)
                    .ConfigureSelfTransition(LexemesTable.GetLexemeId("{"), null, 21);

            states.Add(state.Id, state);

            state = new State(10, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId(")"), 0, null, 3);
            states.Add(state.Id, state);

            state = new State(19, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId(";"), 3);
            states.Add(state.Id, state);

            state = new State(18, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("rof"), 19);
            states.Add(state.Id, state);

            state = new State(17, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId(")"), 0, null, 18);
            states.Add(state.Id, state);

            #endregion

            CurrentState = states[0];
        }

        public void NextState(uint tokenType)
        {
            int nextStateId;
            while (CurrentState.IsFinal)
            {
                if (stack.Count == 0)
                {
                    CurrentState = states[0];
                    break;
                }

                nextStateId = stack.Pop();
                if (states.ContainsKey(nextStateId))
                {
                    CurrentState = states[nextStateId];
                    continue;
                }

                if (char.IsControl((char)nextStateId))
                {
                    throw new Exception($"Tried to jump to: \'{nextStateId}\', but something went wrong");
                }

                throw new Exception($"Something forgotten in stack: \'{(char)nextStateId}\'");
            }

            var previousState = CurrentState;
            nextStateId = CurrentState.Transit(tokenType);

            if (!states.ContainsKey(nextStateId))
            {
                throw new Exception($"lexeme: {LexemesTable.GetLexemeName(tokenType)}. state: {previousState.Id}");
            }
            CurrentState = states[nextStateId];
        }
    }
}
