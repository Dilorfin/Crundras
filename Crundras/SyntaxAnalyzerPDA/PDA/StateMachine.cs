using System;
using System.Collections.Generic;
using System.Linq;

namespace SyntaxAnalyzerPDA.PDA
{
    public class StateMachine
    {
        private readonly State[] states = new State[5];

        private Stack<int> stack = new Stack<int>();
        public State CurrentState { get; private set; }

        public StateMachine()
        {
            states[4] = new State(4, stack, true);

            states[3] = new State(3, stack);

            states[2] = new State(2, stack)
                .ConfigureTransition(31, null, null, states[3]);

            states[1] = new State(1, stack, false, true)
                .ConfigureTransition(1, null, null, states[2]);

            states[0] = new State(0, stack, false, true)
                .ConfigureTransition(4, null, 4, states[1]);

            CurrentState = states[0];
        }
        
        public void NextState(uint tokenType)
        {
            if (CurrentState.IsFinal)
            {
                if (stack.Count == 0)
                    throw new Exception("smth went wrong. stack is empty");
                
                int nextStateId = stack.Pop();
                var nextState = states.First(s => s.Id == nextStateId);
                CurrentState = nextState;
            }

            CurrentState = CurrentState.Transit(tokenType);
        }

        public bool IsFinished => (stack.Count == 0) && CurrentState.IsFinal;
    }
}
