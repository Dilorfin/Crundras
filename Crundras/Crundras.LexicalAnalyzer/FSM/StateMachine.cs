using System;
using System.Collections.Generic;

namespace Crundras.LexicalAnalyzer.FSM
{
    internal partial class StateMachine
    {
        private readonly Dictionary<int, State> states = new Dictionary<int, State>();
        public State CurrentState { get; private set; }

        public StateMachine()
        {
            Config();
        }

        public void NextState(int charClass)
        {
            // refreshing state in final state
            if (CurrentState.IsFinal)
            {
                CurrentState = states[0];
            }

            var nextStateId = CurrentState.Transit(charClass);

            if (!nextStateId.HasValue)
            {
                throw new Exception("Unconfigured transition");
            }

            // transiting to next state
            CurrentState = states[nextStateId.Value];
        }
    }
}