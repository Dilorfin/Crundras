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
