using System;
using System.IO;

namespace Crundras
{
    class Program
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

            try
            {
                var tokensTable = new LexicalAnalyzer.LexicalAnalyzer().Analyze(args[0]);

                foreach (var token in tokensTable.TokensList)
                {
                    Console.Write($"{token.Line,3} {token.Lexeme,10} {token.Code,3}");
                    if (token.ForeignId != 0)
                    {
                        Console.Write($"{token.ForeignId,3}");
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
