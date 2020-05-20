namespace Crundras.LexicalAnalyzer.FSM
{
    internal partial class StateMachine
    {
        private void Config()
        {
            // final for single character lexemes
            states[1] = new State(1, true);

            // numbers
            states[6] = new State(6, true, false);
            states[7] = new State(7, true, false);

            states[202] = new ErrorState(202, "Unexpected character");

            states[5] = new State(5)
                .ConfigureSelfTransition(2)
                .ConfigureTransition(1, 202)
                .ConfigureOtherwiseTransition(6);

            states[4] = new State(4)
                .ConfigureSelfTransition(2)
                .ConfigureTransition('.', 5)
                .ConfigureTransition(1, 202)
                .ConfigureOtherwiseTransition(7);

            // identifiers
            states[3] = new State(3, true, false);

            states[2] = new State(2)
                .ConfigureSelfTransition(1)
                .ConfigureSelfTransition(2)
                .ConfigureOtherwiseTransition(3);

            // == & =
            states[9] = new State(9, true);
            states[10] = new State(10, true, false);

            states[8] = new State(8)
                .ConfigureTransition('=', 9)
                .ConfigureOtherwiseTransition(10);

            // <= & <
            states[12] = new State(12, true);
            states[13] = new State(13, true, false);

            states[11] = new State(11)
                .ConfigureTransition('=', 12)
                .ConfigureOtherwiseTransition(13);

            // >= & >
            states[15] = new State(15, true);
            states[16] = new State(16, true, false);

            states[14] = new State(14)
                .ConfigureTransition('=', 15)
                .ConfigureOtherwiseTransition(16);

            // !=
            states[18] = new State(18, true);

            states[201] = new ErrorState(201, "Expected '='");
            states[17] = new State(17)
                .ConfigureTransition('=', 18)
                .ConfigureOtherwiseTransition(201);

            // ** & *
            states[20] = new State(20, true);
            states[21] = new State(21, true, false);

            states[19] = new State(19)
                .ConfigureTransition('*', 20)
                .ConfigureOtherwiseTransition(21);

            // comments
            states[204] = new ErrorState(204, "Expected end of the comment (\"*/\")");

            states[28] = new State(28, true);

            states[27] = new State(27)
                .ConfigureTransition('\n', 28)
                .ConfigureTransition('\r', 28)
                .ConfigureOtherwiseSelfTransition();

            states[26] = new State(26, true);

            states[25] = new State(25)
                .ConfigureTransition('\0', 204)
                .ConfigureTransition('/', 26)
                .ConfigureOtherwiseTransition(24);

            states[24] = new State(24)
                .ConfigureTransition('\0', 204)
                .ConfigureTransition('*', 25)
                .ConfigureOtherwiseSelfTransition();

            states[23] = new State(23, true, false);

            states[22] = new State(22)
                .ConfigureTransition('*', 24)
                .ConfigureTransition('/', 27)
                .ConfigureOtherwiseTransition(23);

            states[200] = new ErrorState(200, "Unknown symbol");

            // I am root
            states[0] = new State(0)
                .ConfigureSelfTransition(3)
                .ConfigureSelfTransition('\0')
                .ConfigureSelfTransition('\r')
                .ConfigureSelfTransition('\n')
                .ConfigureTransition('$', 1)
                .ConfigureTransition('@', 1)
                .ConfigureTransition('+', 1)
                .ConfigureTransition('-', 1)
                .ConfigureTransition('%', 1)
                .ConfigureTransition('(', 1)
                .ConfigureTransition(')', 1)
                .ConfigureTransition('{', 1)
                .ConfigureTransition('}', 1)
                .ConfigureTransition(';', 1)
                .ConfigureTransition(1, 2)
                .ConfigureTransition(2, 4)
                .ConfigureTransition('=', 8)
                .ConfigureTransition('<', 11)
                .ConfigureTransition('>', 14)
                .ConfigureTransition('!', 17)
                .ConfigureTransition('*', 19)
                .ConfigureTransition('/', 22)
                .ConfigureOtherwiseTransition(200);

            CurrentState = states[0];
        }
    }
}