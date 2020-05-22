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
            #region EXPRESSION
            states[105] = new State(105, stack, true, false);

            states[103] = new State(103, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("("), 106, null, ')')
                .ConfigureTransition(LexemesTable.GetLexemeId("+"), 102)
                .ConfigureTransition(LexemesTable.GetLexemeId("-"), 102)
                .ConfigureTransition(LexemesTable.GetLexemeId("identifier"), 104)
                .ConfigureTransition(LexemesTable.GetLexemeId("int_literal"), 104)
                .ConfigureTransition(LexemesTable.GetLexemeId("float_literal"), 104);

            states[104] = new State(104, stack)
                .ConfigureSelfTransition(LexemesTable.GetLexemeId(")"), ')')
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
                .ConfigureTransition(27, 103)
                .ConfigureTransition(28, 103)
                .ConfigureOtherwiseTransition(105);

            states[102] = new State(102, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("("), 106, null, ')')
                .ConfigureTransition(LexemesTable.GetLexemeId("identifier"), 104)
                .ConfigureTransition(LexemesTable.GetLexemeId("int_literal"), 104)
                .ConfigureTransition(LexemesTable.GetLexemeId("float_literal"), 104);

            states[106] = new State(106, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("+"), 102)
                .ConfigureTransition(LexemesTable.GetLexemeId("-"), 102)
                .ConfigureTransition(LexemesTable.GetLexemeId("identifier"), 101)
                .ConfigureTransition(LexemesTable.GetLexemeId("int_literal"), 101)
                .ConfigureTransition(LexemesTable.GetLexemeId("float_literal"), 101)
                .ConfigureSelfTransition(LexemesTable.GetLexemeId("("), null, ')');

            states[101] = new State(101, stack, false, false)
                .ConfigureTransition(LexemesTable.GetLexemeId("+"), 102)
                .ConfigureTransition(LexemesTable.GetLexemeId("-"), 102)
                .ConfigureTransition(LexemesTable.GetLexemeId("identifier"), 104)
                .ConfigureTransition(LexemesTable.GetLexemeId("int_literal"), 104)
                .ConfigureTransition(LexemesTable.GetLexemeId("float_literal"), 104)
                .ConfigureTransition(LexemesTable.GetLexemeId("("), 106, null, ')');

            states[100] = new State(100, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("+"), 101)
                .ConfigureTransition(LexemesTable.GetLexemeId("-"), 101)
                .ConfigureTransition(LexemesTable.GetLexemeId("identifier"), 101)
                .ConfigureTransition(LexemesTable.GetLexemeId("int_literal"), 101)
                .ConfigureTransition(LexemesTable.GetLexemeId("float_literal"), 101)
                .ConfigureTransition(LexemesTable.GetLexemeId("("), 101);

            #endregion

            #region STATEMENT
            states[3] = new State(3, stack, true);

            // for
            states[21] = new State(21, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId(";"), 3);

            states[20] = new State(20, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("rof"), 21);

            states[19] = new State(19, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId(")"), 0, null, 20);

            states[18] = new State(18, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("("), 100, null, 19);

            states[17] = new State(17, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("while"), 18);

            states[16] = new State(16, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("by"), 100, null, 17);

            states[15] = new State(15, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("to"), 100, null, 16);

            states[14] = new State(14, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("="), 100, null, 15);

            states[13] = new State(13, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("identifier"), 14);

            // assignment
            states[12] = new State(12, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId(";"), 3);
            // goto & assignment
            states[11] = new State(11, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("="), 100, null, 12)
                .ConfigureTransition(LexemesTable.GetLexemeId(":"), 3);

            // if
            states[10] = new State(10, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId(")"), 0, null, 3);

            states[9] = new State(9, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("("), 100, null, 10);

            // goto
            states[8] = new State(8, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId(";"), 3);

            states[7] = new State(7, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("identifier"), 8);

            // output
            states[6] = new State(6, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId(";"), 3);

            // input
            states[5] = new State(5, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId(";"), 3);

            states[4] = new State(4, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("identifier"), 5);

            // declaration
            states[2] = new State(2, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId(";"), 3);

            states[1] = new State(1, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("identifier"), 2);

            // root
            states[0] = new State(0, stack)
                .ConfigureTransition(LexemesTable.GetLexemeId("int"), 1)
                .ConfigureTransition(LexemesTable.GetLexemeId("float"), 1)
                .ConfigureTransition(LexemesTable.GetLexemeId("$"), 4)
                .ConfigureTransition(LexemesTable.GetLexemeId("@"), 100, null, 6)
                .ConfigureTransition(LexemesTable.GetLexemeId("goto"), 7)
                .ConfigureTransition(LexemesTable.GetLexemeId("identifier"), 11)
                .ConfigureTransition(LexemesTable.GetLexemeId("if"), 9)
                .ConfigureTransition(LexemesTable.GetLexemeId("for"), 13)
                .ConfigureTransition(LexemesTable.GetLexemeId("}"), 3)
                .ConfigureSelfTransition(LexemesTable.GetLexemeId("{"));

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
                if (!states.ContainsKey(nextStateId))
                {
                    throw new Exception($"Something forgotten in stack: \'{(char)nextStateId}\'");
                }

                CurrentState = states[nextStateId];
            }

            nextStateId = CurrentState.Transit(tokenType);

            if (!states.ContainsKey(nextStateId))
            {
                throw new Exception($"lexeme: {LexemesTable.GetLexemeName(tokenType)}. state: {CurrentState.Id}");
            }
            CurrentState = states[nextStateId];
        }
    }
}
