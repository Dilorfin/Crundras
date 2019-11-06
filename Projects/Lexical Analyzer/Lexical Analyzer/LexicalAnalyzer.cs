using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Lexical_Analyzer
{
    class LexicalAnalyzer
    {
        private StateMachine stateMachine = new StateMachine();
        private TokenTable tokenTable = new TokenTable();

        private uint line = 1;

        public TokenTable Analyze(string fileName)
        {
            using StreamReader file = new StreamReader(fileName, true);
            
            StringBuilder builder = new StringBuilder();

            while (!file.EndOfStream)
            {
                char nextChar = (char) file.Peek();

                int charClass = GetCharClass(nextChar);
                stateMachine.NextState(charClass);

                CheckError(stateMachine.CurrentState, nextChar, line);

                if (nextChar == '\n') 
                    line++;

                if (stateMachine.CurrentState.TakeCharacter)
                {
                    builder.Append((char) file.Read());
                }
                if (stateMachine.CurrentState.IsFinal)
                {
                    AddLexeme(builder.ToString());
                    builder.Clear();
                }

                if (stateMachine.CurrentState.Id == 0)
                {
                    builder.Clear();
                }
            }

            // '\0' - last character
            char lastChar = '\0';
            stateMachine.NextState(lastChar);
            CheckError(stateMachine.CurrentState, lastChar, line);

            if (stateMachine.CurrentState.IsFinal)
            {
                AddLexeme(builder.ToString());
                builder.Clear();
            }

            file.Close();

            return tokenTable;
        }

        private void AddLexeme(string lexeme)
        {
            tokenTable.AddToken(line, lexeme, stateMachine.CurrentState.Id);
        }

        private void CheckError(State state, char nextChar, uint line)
        {
            if (!state.IsError) 
                return;

            // character escaping
            string character = Regex.Escape(new string(nextChar, 1));
            // displaying error message
            string message = (state as ErrorState)?.Message;

            throw new Exception($"{message}. Character: '{character}'. Line: {line}.");
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