using Crundras.Common;
using Crundras.Common.Tables;
using System;
using System.IO;

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
                var tables = LexicalAnalyzer.AnalyzeFile(args[0]);

                foreach (var token in tables.TokenTable)
                {
                    Console.Write($"{token.Line,4} {LexemesTable.GetLexemeName(token.Code),20} {token.Code,10}");
                    if (token.ForeignId.HasValue)
                    {
                        if (Token.IsIdentifier(token.Code))
                        {
                            Console.Write($"\"{tables.IdentifiersTable[token.ForeignId.Value].Name}\"");
                        }
                        else if (Token.IsIntLiteral(token.Code))
                        {
                            Console.Write($"\"{tables.IntLiteralsTable[token.ForeignId.Value]}\"");
                        }
                        else if (Token.IsFloatLiteral(token.Code))
                        {
                            Console.Write($"\"{tables.FloatLiteralsTable[token.ForeignId.Value]}\"");
                        }
                        Console.Write($"({token.ForeignId.Value})");
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
