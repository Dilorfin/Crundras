namespace LexicalAnalyzer
{
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