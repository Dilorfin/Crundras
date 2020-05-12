using Crundras.Common;
using Crundras.LexicalAnalyzer;
using SyntaxAnalyzerPDA;
using System;
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
                foreach (var rpnToken in RPNTranslator.Analyze(syntaxTree))
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }
    }
}
