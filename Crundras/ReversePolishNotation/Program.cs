using LexicalAnalyzer;
using System;
using System.Collections.Generic;
using System.IO;

namespace ReversePolishNotation
{
    internal class SyntaxAnalyzerRPN
    {
        private Stack<Token> stack = new Stack<Token>();
        private LinkedList<Token> result = new LinkedList<Token>();

        private readonly Dictionary<uint, int> priorityTable = new Dictionary<uint, int>
        {
            { TokenTable.GetLexemeId("("), 0 }
        };

        void NextToken(Token token)
        {

        }

        public static void Analyze(TokenTable tokenTable)
        {
            var syntaxAnalyzerRpn = new SyntaxAnalyzerRPN();
            foreach (var token in tokenTable.TokensList)
            {
                syntaxAnalyzerRpn.NextToken(token);
            }
        }
    }

    internal static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Crundras.exe %filename%");
                return;
            }

            if (!File.Exists(args[0]))
            {
                Console.WriteLine($"File \"{args[0]}\" doesn't seem to exist.");
                return;
            }

            var tokenTable = new LexicalAnalyzer.LexicalAnalyzer().Analyze(args[0]);

            SyntaxAnalyzerRPN.Analyze(tokenTable);


            Console.ReadKey();
        }
    }
}
