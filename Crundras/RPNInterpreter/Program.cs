using Crundras.LexicalAnalyzer;
using SyntaxAnalyzerPDA;
using System;
using System.IO;

namespace RPNInterpreter
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
                var tables = LexicalAnalyzer.AnalyzeFile(args[0]);

                var syntaxTree = SyntaxAnalyzer.Analyze(tables);
                var rpnTokens = RPNTranslator.RPNTranslator.Analyze(tables, syntaxTree);

                Console.WriteLine("Interpreting:");
                new RPNInterpreter().Interpret(tables, rpnTokens);

                tables.IdentifiersTable.Display();
                tables.IntLiteralsTable.Display();
                tables.FloatLiteralsTable.Display();
                tables.LabelsTable.Display();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }
    }
}
