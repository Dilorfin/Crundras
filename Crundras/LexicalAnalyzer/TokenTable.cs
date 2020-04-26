using System.Collections.Generic;
using System.Linq;

namespace LexicalAnalyzer
{
    public class TokenTable
    {
        public static readonly Dictionary<uint, string> CodesTable = new Dictionary<uint, string>
        {
            { 4, "int"},
            { 5, "float"},
            { 6, "if"},
            { 7, "for"},
            { 8, "to"},
            { 9, "by"},
            { 10, "while"},
            { 11, "rof"},
            { 12, "$"},
            { 13, "@"},
            { 14, "="},
            { 15, "+"},
            { 16, "-"},
            { 17, "*"},
            { 18, "**"},
            { 19, "/"},
            { 20, "%"},
            { 21, "<"},
            { 22, ">"},
            { 23, "<="},
            { 24, "=="},
            { 25, ">="},
            { 26, "!="},
            { 27, "("},
            { 28, ")"},
            { 29, "{"},
            { 30, "}"},
            { 31, ";"}
        };
        public struct Token
        {
            /// <summary>
            /// line of source file where was current token
            /// </summary>
            public uint Line;
            /// <summary>
            /// just lexeme
            /// </summary>
            public string Lexeme;
            /// <summary>
            /// token code in tokens table in specification
            /// </summary>
            public uint Code;
            /// <summary>
            /// id of lexeme in otherwise table
            /// </summary>
            public uint ForeignId;
        }

        public readonly LinkedList<Token> TokensList = new LinkedList<Token>();

        public readonly Dictionary<uint, string> LiteralsTable = new Dictionary<uint, string>();
        public readonly Dictionary<uint, string> IdentifiersTable = new Dictionary<uint, string>();

        public void AddToken(uint line, string lexeme, int stateId)
        {
            var token = new Token { Line = line, Lexeme = lexeme, ForeignId = 0 };

            // checking if lexeme is language specific
            if (CodesTable.ContainsValue(lexeme))
            {
                token.Code = CodesTable.First(pair => pair.Value == lexeme).Key;
            }
            else
            {
                // identifiers
                if (stateId == 3)
                {
                    token.Code = 1;
                    if (!IdentifiersTable.ContainsValue(lexeme))
                    {
                        IdentifiersTable.Add((uint)(IdentifiersTable.Count + 1), lexeme);
                    }
                    token.ForeignId = IdentifiersTable.First(pair => pair.Value == lexeme).Key;
                }
                // (int | float) literals
                else
                {
                    // 2 - int 3 - float
                    token.Code = (uint)(stateId == 7 ? 2 : 3);

                    if (!LiteralsTable.ContainsValue(lexeme))
                    {
                        LiteralsTable.Add((uint)(LiteralsTable.Count + 1), lexeme);
                    }
                    token.ForeignId = LiteralsTable.First(pair => pair.Value == lexeme).Key;
                }
            }

            TokensList.AddLast(token);
        }

        public static string GetLexemeName(uint code)
        {
            if (code == 1) return "identifier";
            if (code == 2) return "int_literal";
            if (code == 3) return "float_literal";
            return CodesTable[code];
        }

        public static uint GetLexemeId(string lexeme)
        {
            if (lexeme == "identifier") return 1;
            if (lexeme == "int_literal") return 2;
            if (lexeme == "float_literal") return 3;
            return CodesTable.First(p => p.Value == lexeme).Key;
        }
    }
}