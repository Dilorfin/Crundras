using System.Collections.Generic;
using System.Linq;

namespace Crundras.Common.Tables
{
    public static class LexemesTable
    {
        private static readonly Dictionary<uint, string> CodesTable = new Dictionary<uint, string>
        {
            { 1, "identifier"},
            { 2, "label"},
            { 3, "int_literal"},
            { 4, "float_literal"},
            { 5, "int"},
            { 6, "float"},
            { 7, "if"},
            { 8, "for"},
            { 9, "to"},
            { 10, "by"},
            { 11, "while"},
            { 12, "rof"},
            { 13, "goto"},
            { 14, "$"},
            { 15, "@"},
            { 16, "="},
            { 17, "+"},
            { 18, "-"},
            { 19, "*"},
            { 20, "**"},
            { 21, "/"},
            { 22, "%"},
            { 23, "<"},
            { 24, ">"},
            { 25, "<="},
            { 26, "=="},
            { 27, ">="},
            { 28, "!="},
            { 29, "("},
            { 30, ")"},
            { 31, "{"},
            { 32, "}"},
            { 33, ";"},
            { 34, ":"}
        };

        public static bool IsKeyword(string lexeme)
        {
            var lexemeId = GetLexemeId(lexeme);
            return lexemeId >= 5 && lexemeId <= 34;
        }

        public static string GetLexemeName(uint code)
        {
            return CodesTable[code];
        }

        public static uint GetLexemeId(string lexeme)
        {
            if (!CodesTable.ContainsValue(lexeme))
            {
                return 0;
            }

            return CodesTable.First(p => p.Value == lexeme).Key;
        }
    }
}