namespace LexicalAnalyzer
{
    class StateMachine
    {
        private readonly State[] states = new State[22];
        private readonly State initialState;
        public State CurrentState { get; private set; }

        public StateMachine()
        {
            // letter - 1, digit - 2
            states[1] = new State(1, true);

            // numbers
            states[6] = new State(6, true, false);
            states[7] = new State(7, true, false);

            states[5] = new State(5)
                .ConfigureSelfTransition(2)
                .ConfigureTransition(1, new ErrorState(202, "Unexpected character"))
                .ConfigureOtherwiseTransition(states[6]);

            states[4] = new State(4)
                .ConfigureSelfTransition(2)
                .ConfigureTransition('.', states[5])
                .ConfigureTransition(1, new ErrorState(202, "Unexpected character"))
                .ConfigureOtherwiseTransition(states[7]);

            // identifiers
            states[3] = new State(3, true, false);

            states[2] = new State(2)
                .ConfigureSelfTransition(1)
                .ConfigureSelfTransition(2)
                .ConfigureOtherwiseTransition(states[3]);

            // == & =
            states[9] = new State(9, true);
            states[10] = new State(10, true, false);

            states[8] = new State(8)
                .ConfigureTransition('=', states[9])
                .ConfigureOtherwiseTransition(states[10]);

            // <= & <
            states[12] = new State(12, true);
            states[13] = new State(13, true, false);

            states[11] = new State(11)
                .ConfigureTransition('=', states[12])
                .ConfigureOtherwiseTransition(states[13]);

            // >= & >
            states[15] = new State(15, true);
            states[16] = new State(16, true, false);

            states[14] = new State(14)
                .ConfigureTransition('=', states[15])
                .ConfigureOtherwiseTransition(states[16]);

            // !=
            states[18] = new State(18, true);

            states[17] = new State(17)
                .ConfigureTransition('=', states[18])
                .ConfigureOtherwiseTransition(new ErrorState(201, "Expected '='"));

            // ** & *
            states[20] = new State(20, true);
            states[21] = new State(21, true, false);

            states[19] = new State(19)
                .ConfigureTransition('*', states[20])
                .ConfigureOtherwiseTransition(states[21]);

            // I am root
            states[0] = new State(0)
                .ConfigureSelfTransition(3)
                .ConfigureSelfTransition('\0')
                .ConfigureTransition('$', states[1])
                .ConfigureTransition('@', states[1])
                .ConfigureTransition('+', states[1])
                .ConfigureTransition('-', states[1])
                .ConfigureTransition('/', states[1])
                .ConfigureTransition('%', states[1])
                .ConfigureTransition('(', states[1])
                .ConfigureTransition(')', states[1])
                .ConfigureTransition('{', states[1])
                .ConfigureTransition('}', states[1])
                .ConfigureTransition(';', states[1])
                .ConfigureTransition(1, states[2])
                .ConfigureTransition(2, states[4])
                .ConfigureTransition('=', states[8])
                .ConfigureTransition('<', states[11])
                .ConfigureTransition('>', states[14])
                .ConfigureTransition('!', states[17])
                .ConfigureTransition('*', states[19])
                .ConfigureOtherwiseTransition(new ErrorState(200, "Unknown symbol"));

            initialState = states[0];
            CurrentState = initialState;
        }

        public void NextState(int charClass)
        {
            // refreshing state in final state
            if (CurrentState.IsFinal)
            {
                CurrentState = initialState;
            }

            // transiting to next state
            CurrentState = CurrentState.Transit(charClass);
        }
    }
}