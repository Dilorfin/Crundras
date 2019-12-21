using System.Collections.Generic;

namespace SyntaxAnalyzerPDA.PDA
{
    public class State
    {
        private class TransitionUnit
        {
            public TransitionUnit(int? pop, int? push, State state)
            {
                Pop = pop;
                Push = push;
                State = state;
            }

            public int? Pop { get; }
            public int? Push { get; }

            public State State { get; }
        }

        public int Id { get; }

        private readonly Stack<int> stack;

        private readonly Dictionary<uint, TransitionUnit> transitions = new Dictionary<uint, TransitionUnit>();
        private TransitionUnit otherwise = null;

        public State(int id, Stack<int> stack, bool isFinal = false)
        {
            this.Id = id;
            this.stack = stack;
            this.IsFinal = isFinal;
        }

        public State ConfigureTransition(uint tokenType, int? pop, int? push, State nextState)
        {
            if (CanTransit(tokenType))
                return this;
            
            var transitionUnit = new TransitionUnit(pop, push, nextState);

            transitions.Add(tokenType, transitionUnit);

            return this;
        }
        public State ConfigureSelfTransition(uint tokenType, int? pop, int? push)
        {
            if (CanTransit(tokenType)) 
                return this;

            var transitionUnit = new TransitionUnit(pop, push, this);

            transitions.Add(tokenType, transitionUnit);

            return this;
        }
        public State ConfigureOtherwiseTransition(int? pop, int? push, State state)
        {
            otherwise = new TransitionUnit(pop, push, state);
            return this;
        }

        public State Transit(uint tokenType)
        {
            if (!CanTransit(tokenType)) 
                return otherwise.State;

            var transitionUnit = transitions[tokenType];

            stack.PopValue(transitionUnit.Pop);
            stack.PushValue(transitionUnit.Push);

            return transitionUnit.State;
        }

        private bool CanTransit(uint tokenType)
        {
            return transitions.ContainsKey(tokenType);
        }

        public bool IsError => this.GetType() == typeof(ErrorState);
        public bool IsFinal { get; }
    }
}