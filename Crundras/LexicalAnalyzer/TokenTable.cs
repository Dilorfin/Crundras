using System.Collections.Generic;
using System.Linq;

namespace LexicalAnalyzer
{
    public class TokenTable
    {
        public static readonly Dictionary<uint, string> codesTable = new Dictionary<uint, string>
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
            public uint line;
            public string lexeme;
            public uint code;
            public uint id;
        }

        public LinkedList<Token> tokensList = new LinkedList<Token>();

        public Dictionary<uint, string> literalsTable = new Dictionary<uint, string>();
        public Dictionary<uint, string> identifiersTable = new Dictionary<uint, string>();

        public void AddToken(uint line, string lexeme, int stateId)
        {
            var token = new Token { line = line, lexeme = lexeme, id = 0 };
            
            // checking if lexeme is language specific
            if (codesTable.ContainsValue(lexeme))
            {
                token.code = codesTable.First(pair => pair.Value == lexeme).Key;
            }
            else
            {
                // identifiers
                if (stateId == 3)
                {
                    token.code = 1;
                    if (!identifiersTable.ContainsValue(lexeme))
                    {
                        identifiersTable.Add((uint)(identifiersTable.Count+1), lexeme);
                    }
                    token.id = identifiersTable.First(pair => pair.Value == lexeme).Key;
                }
                // (int | float) literals
                else
                {
                    // 2 - int 3 - float
                    token.code = (uint) (stateId == 7? 2 : 3);

                    if (!literalsTable.ContainsValue(lexeme))
                    {
                        literalsTable.Add((uint)(literalsTable.Count+1), lexeme);
                    }
                    token.id = literalsTable.First(pair => pair.Value == lexeme).Key;
                }
            }

            tokensList.AddLast(token);
        }

        public static string GetLexemeName(uint code)
        {
            if (code == 1) return "identifier";
            if (code == 2) return "int";
            if (code == 3) return "float";
            return codesTable[code];
        }
    }
}