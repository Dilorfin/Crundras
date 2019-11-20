using System.Collections.Generic;

namespace LexicalAnalyzer
{
    class State
    {
        public int Id { get; }

        private Dictionary<int, State> transitions = new Dictionary<int, State>();
        private State otherwise = null;

        public State(int id, bool isFinal = false, bool takeCharacter = true)
        {
            this.Id = id;
            this.IsFinal = isFinal;
            this.TakeCharacter = takeCharacter;
        }

        public State ConfigureTransition(int charClass, State nextState)
        {
            if (!CanTransit(charClass))
            {
                transitions.Add(charClass, nextState);
            }

            return this;
        }
        public State ConfigureSelfTransition(int charClass)
        {
            if (!CanTransit(charClass))
            {
                transitions.Add(charClass, this);
            }

            return this;
        }
        public State ConfigureOtherwiseTransition(State state)
        {
            otherwise = state;
            return this;
        }

        public State Transit(int charClass)
        {
            if (CanTransit(charClass))
                return transitions[charClass];
            return otherwise;
        }

        private bool CanTransit(int charClass)
        {
            return transitions.ContainsKey(charClass);
        }

        public bool IsError => this.GetType() == typeof(ErrorState);
        public bool IsFinal { get; }
        public bool TakeCharacter { get;}
    }

    class ErrorState : State
    {
        public string Message { get; }

        public ErrorState(int id, string message)
            : base(id, true, false)
        {
            this.Message = message;
        }
    }
}