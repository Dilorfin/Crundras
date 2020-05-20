using System.Collections.Generic;
using System.Linq;

namespace Crundras.Common.Tables
{
    public static class LexemesTable
    {
        private static readonly Dictionary<uint, string> CodesTable = new Dictionary<uint, string>
        {
            { 1, "identifier"},
            { 2, "int_literal"},
            { 3, "float_literal"},
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
            { 31, ";"},
            { 32, "goto"}
        };

        public static bool IsKeyword(string lexeme)
        {
            var lexemeId = GetLexemeId(lexeme);
            return lexemeId >= 4 && lexemeId <= 31;
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