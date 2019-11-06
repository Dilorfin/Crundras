using System;
using System.IO;

namespace Lexical_Analyzer
{
    class Program
    {
        static void Main(string[] args)
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
                var tokensTable = new LexicalAnalyzer().Analyze(args[0]);

                foreach (var token in tokensTable.tokensList)
                {
                    Console.Write($"{token.line, 3} {token.lexeme, 10} {token.code, 3}");
                    if (token.id != 0)
                    {
                        Console.Write($"{token.id,3}");
                    }
                    Console.WriteLine();
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
