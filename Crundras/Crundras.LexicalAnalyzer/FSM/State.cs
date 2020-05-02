using System.Collections.Generic;

namespace Crundras.LexicalAnalyzer.FSM
{
    internal class State
    {
        public int Id { get; }

        private readonly Dictionary<int, int> transitions = new Dictionary<int, int>();
        private int? otherwiseId;

        public State(int id, bool isFinal = false, bool takeCharacter = true)
        {
            this.Id = id;
            this.IsFinal = isFinal;
            this.TakeCharacter = takeCharacter;
        }

        public State ConfigureTransition(int charClass, int nextStateId)
        {
            if (!CanTransit(charClass))
            {
                transitions.Add(charClass, nextStateId);
            }

            return this;
        }
        
        public State ConfigureOtherwiseTransition(int nextStateId)
        {
            otherwiseId = nextStateId;
            return this;
        }

        public State ConfigureSelfTransition(int charClass)
        {
            return ConfigureTransition(charClass, this.Id);
        }
        public State ConfigureOtherwiseSelfTransition()
        {
            return ConfigureOtherwiseTransition(this.Id);
        }

        public int? Transit(int charClass)
        {
            if (CanTransit(charClass))
            {
                return transitions[charClass];
            }

            return otherwiseId;
        }

        private bool CanTransit(int charClass)
        {
            return transitions.ContainsKey(charClass);
        }

        public bool IsError => this.GetType() == typeof(ErrorState);
        public bool IsFinal { get; }
        public bool TakeCharacter { get; }
    }
}