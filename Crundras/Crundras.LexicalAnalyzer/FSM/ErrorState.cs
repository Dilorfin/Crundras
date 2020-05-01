namespace Crundras.LexicalAnalyzer.FSM
{
    internal class ErrorState : State
    {
        public string Message { get; }

        public ErrorState(int id, string message)
            : base(id, true, false)
        {
            this.Message = message;
        }
    }
}