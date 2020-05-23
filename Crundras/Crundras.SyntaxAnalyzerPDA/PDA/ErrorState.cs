namespace SyntaxAnalyzerPDA.PDA
{
    public class ErrorState : State
    {
        public string Message { get; }

        public ErrorState(int id, string message)
            : base(id, null, true)
        {
            this.Message = message;
        }
    }
}