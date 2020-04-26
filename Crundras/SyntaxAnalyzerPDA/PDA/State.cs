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

        public State(int id, Stack<int> stack, bool isFinal = false, bool takeToken = true)
        {
            this.Id = id;
            this.stack = stack;
            this.TakeToken = takeToken;
            this.IsFinal = isFinal;
        }

        #region CONFIGS
        public State ConfigureTransition(uint tokenType, State nextState, int? pop = null, int? push = null)
        {
            if (CanTransit(tokenType))
                return this;

            var transitionUnit = new TransitionUnit(pop, push, nextState);

            transitions.Add(tokenType, transitionUnit);

            return this;
        }
        public State ConfigureSelfTransition(uint tokenType, int? pop = null, int? push = null)
        {
            if (CanTransit(tokenType))
                return this;

            var transitionUnit = new TransitionUnit(pop, push, this);

            transitions.Add(tokenType, transitionUnit);

            return this;
        }
        public State ConfigureOtherwiseTransition(State state, int? pop = null, int? push = null)
        {
            otherwise = new TransitionUnit(pop, push, state);
            return this;
        }
        #endregion

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
        public bool IsLevelStart => new List<int> { 0, 100, 1, 4, 7, 9, 11 }.Contains(Id);
        public bool IsFinal { get; }
        public bool TakeToken { get; }
    }
}