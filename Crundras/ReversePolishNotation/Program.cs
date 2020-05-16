using Crundras.Common;
using Crundras.LexicalAnalyzer;
using SyntaxAnalyzerPDA;
using System;
using System.Collections.Generic;
using System.IO;

namespace ReversePolishNotation
{
    internal static class Program
    {
        private static void Main(string[] args)
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

            try
            {
                var tokensTable = LexicalAnalyzer.AnalyzeFile(args[0]);

                var syntaxTree = SyntaxAnalyzer.Analyze(tokensTable);
                var rpnTokens = RPNTranslator.Analyze(syntaxTree);
                new RPNArithmeticInterpreter().Interpret(tokensTable, rpnTokens);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }

        private static void DisplayRpnTokens(TokenTable tokensTable, LinkedList<RPNToken> rpnTokens)
        {
            foreach (var rpnToken in rpnTokens)
            {
                if (rpnToken.Id.HasValue)
                {
                    Console.Write(Token.IsIdentifier(rpnToken.LexemeCode)
                        ? tokensTable.IdentifiersTable[rpnToken.Id.Value]
                        : tokensTable.LiteralsTable[rpnToken.Id.Value]);
                }
                else Console.Write(rpnToken.Name);
                Console.Write(" ");
            }
        }
    }
}
