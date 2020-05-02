using Crundras.Common;
using Crundras.LexicalAnalyzer.FSM;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Crundras.LexicalAnalyzer
{
    public class LexicalAnalyzer
    {
        private readonly StringBuilder stringBuilder;
        private readonly StateMachine stateMachine;
        private State CurrentState => stateMachine.CurrentState;

        public TokenTable TokenTable { get; }

        private uint line = 1;
        
        public LexicalAnalyzer()
        {
            this.TokenTable = new TokenTable();
            this.stateMachine = new StateMachine();
            this.stringBuilder = new StringBuilder();
        }

        public void NextChar(char nextChar)
        {
            do
            {
                int charClass = GetCharClass(nextChar);
                // transiting state machine to next state
                stateMachine.NextState(charClass);

                // checking if error has occurred
                if (CurrentState.IsError)
                {
                    // character escaping
                    string character = Regex.Escape(new string(nextChar, 1));
                    // displaying error message
                    string message = (CurrentState as ErrorState)?.Message;

                    throw new Exception($"{message}. Character: '{character}'. Line: {line}.");
                }

                // counting lines
                if (nextChar == '\n')
                {
                    line++;
                }

                // checking for '*' states
                if (CurrentState.TakeCharacter)
                {
                    stringBuilder.Append(nextChar);
                }
                // adding lexeme to table in final states
                if (CurrentState.IsFinal)
                {
                    var lexeme = stringBuilder.ToString();
                    TokenTable.AddToken(line, lexeme, CurrentState.Id);
                    stringBuilder.Clear();
                }

                // clearing builder at 0 state, needed in case of self transiting
                if (CurrentState.Id == 0)
                {
                    stringBuilder.Clear();
                }
            } 
            while (!CurrentState.TakeCharacter);
        }

        public static TokenTable AnalyzeFile(string fileName)
        {
            using var file = new StreamReader(fileName, true);
            var analyzer = new LexicalAnalyzer();
            
            while (!file.EndOfStream)
            {
                analyzer.NextChar((char) file.Read());
            }

            // '\0' - last character
            analyzer.NextChar('\0');

            file.Close();

            return analyzer.TokenTable;
        }
        
        private static int GetCharClass(char c)
        {
            int charClass = c;
            if (char.IsLetter(c))
            {
                charClass = 1;
            }
            else if (char.IsDigit(c))
            {
                charClass = 2;
            }
            else if (char.IsWhiteSpace(c))
            {
                charClass = 3;
            }

            return charClass;
        }
    }
}