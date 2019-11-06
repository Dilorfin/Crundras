using System.Collections.Generic;

namespace Lexical_Analyzer
{
    class TokenTable
    {
        public static readonly Dictionary<string, uint> codesTable = new Dictionary<string, uint>
        {
            { "int", 4 },
            { "float", 5 },
            { "if", 6 },
            { "for", 7 },
            { "to", 8 },
            { "by", 9 },
            { "while", 10 },
            { "rof", 11 },
            { "$", 12 },
            { "@", 13 },
            { "=", 14 },
            { "+", 15 },
            { "-", 16 },
            { "*", 17 },
            { "**", 18 },
            { "/", 19 },
            { "%", 20 },
            { "<", 21 },
            { ">", 22 },
            { "<=", 23 },
            { "==", 24 },
            { ">=", 25 },
            { "!=", 26 },
            { "(", 27 },
            { ")", 28 },
            { "{", 29 },
            { "}", 30 },
            { ";", 31 }
        };

        public struct Token
        {
            public uint line;
            public string lexeme;
            public uint code;
            public uint id;
        }

        public LinkedList<Token> tokensList = new LinkedList<Token>();

        public Dictionary<string, uint> literalsTable = new Dictionary<string, uint>();
        public Dictionary<string, uint> identifiersTable = new Dictionary<string, uint>();

        public void AddToken(uint line, string lexeme, int stateId)
        {
            Token token = new Token
            {
                line = line, 
                lexeme = lexeme,
                id = 0
            };

            if (codesTable.ContainsKey(lexeme))
            {
                token.code = codesTable[lexeme];
            }
            else
            {
                // identifiers
                if (stateId == 3)
                {
                    token.code = 1;
                    if (!identifiersTable.ContainsKey(lexeme))
                    {
                        identifiersTable.Add(lexeme, (uint)(identifiersTable.Count+1));
                    }
                    token.id = identifiersTable[lexeme];
                }
                // float
                else
                {
                    token.code = (uint) (stateId == 6? 3 : 2);

                    if (!literalsTable.ContainsKey(lexeme))
                    {
                        literalsTable.Add(lexeme, (uint)(literalsTable.Count+1));
                    }
                    token.id = literalsTable[lexeme];
                }
            }

            tokensList.AddLast(token);
        }
    }
}