using System;
using System.IO;
using Crundras.Common;

namespace Crundras.LexicalAnalyzer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Lexical_Analyzer.exe %filename%");
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

                foreach (var token in tokensTable.TokensList)
                {
                    Console.Write($"{token.Line,15} {TokenTable.GetLexemeName(token.Code),40} {token.Code,10}");
                    if (token.ForeignId.HasValue)
                    {
                        Console.Write($"{token.ForeignId.Value,3}");
                    }
                    Console.WriteLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
