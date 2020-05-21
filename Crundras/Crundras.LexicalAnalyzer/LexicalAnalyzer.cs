using Crundras.Common;
using Crundras.Common.Tables;
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

        public TablesCollection Tables { get; }

        private uint line = 1;

        private LexicalAnalyzer()
        {
            this.Tables = new TablesCollection();

            this.stateMachine = new StateMachine();
            this.stringBuilder = new StringBuilder();
        }

        private void NextChar(char nextChar)
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
                    AddLexeme(lexeme, CurrentState.Id);
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

        public static TablesCollection AnalyzeFile(string fileName)
        {
            using var file = new StreamReader(fileName, true);
            var analyzer = new LexicalAnalyzer();

            while (!file.EndOfStream)
            {
                analyzer.NextChar((char)file.Read());
            }

            // '\0' - last character
            analyzer.NextChar('\0');

            file.Close();

            return analyzer.Tables;
        }

        private void AddLexeme(string lexeme, int stateId)
        {
            // skip comments
            if (stateId == 26 || stateId == 28)
            {
                return;
            }

            var token = new Token { Line = line, ForeignId = null };

            // checking if lexeme is language specific
            if (LexemesTable.IsKeyword(lexeme))
            {
                token.Code = LexemesTable.GetLexemeId(lexeme);
            }
            else
            {
                // identifiers
                if (stateId == 3)
                {
                    token.Code = LexemesTable.GetLexemeId("identifier");
                    token.ForeignId = Tables.IdentifiersTable.GetId(lexeme);
                }
                // int literal
                else if (stateId == 7)
                {
                    token.Code = LexemesTable.GetLexemeId("int_literal");
                    token.ForeignId = Tables.IntLiteralsTable.GetId(lexeme);
                }
                // float literal
                else
                {
                    token.Code = LexemesTable.GetLexemeId("float_literal");
                    token.ForeignId = Tables.FloatLiteralsTable.GetId(lexeme);
                }
            }

            Tables.TokenTable.AddLast(token);
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
            else if (char.IsWhiteSpace(c) && !("\r\n").Contains(c))
            {
                charClass = 3;
            }

            return charClass;
        }
    }
}