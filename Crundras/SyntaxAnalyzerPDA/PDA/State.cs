using System.Collections.Generic;

namespace SyntaxAnalyzerPDA.PDA
{
    public class State
    {
        private class TransitionUnit
        {
            public TransitionUnit(int? pop, int? push, int stateId)
            {
                Pop = pop;
                Push = push;
                StateId = stateId;
            }

            public int? Pop { get; }
            public int? Push { get; }

            public int StateId { get; }
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
        public State ConfigureTransition(uint tokenType, int nextStateId, int? pop = null, int? push = null)
        {
            if (CanTransit(tokenType))
            {
                return this;
            }

            var transitionUnit = new TransitionUnit(pop, push, nextStateId);

            transitions.Add(tokenType, transitionUnit);

            return this;
        }
        public State ConfigureSelfTransition(uint tokenType, int? pop = null, int? push = null)
        {
            if (CanTransit(tokenType))
            {
                return this;
            }

            var transitionUnit = new TransitionUnit(pop, push, this.Id);

            transitions.Add(tokenType, transitionUnit);

            return this;
        }
        public State ConfigureOtherwiseTransition(int stateId, int? pop = null, int? push = null)
        {
            otherwise = new TransitionUnit(pop, push, stateId);
            return this;
        }
        #endregion

        public int Transit(uint tokenType)
        {
            if (!CanTransit(tokenType))
            {
                return otherwise.StateId;
            }

            var transitionUnit = transitions[tokenType];
            if (transitionUnit.Pop.HasValue)
            {
                if (stack.Peek() != transitionUnit.Pop.Value)
                {
                    return otherwise.StateId;
                }
                stack.Pop();
            }

            if (transitionUnit.Push.HasValue)
            {
                stack.Push(transitionUnit.Push.Value);
            }

            return transitionUnit.StateId;
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