using Crundras.Common;
using Crundras.Common.Tables;
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
                var tables = LexicalAnalyzer.AnalyzeFile(args[0]);

                var syntaxTree = SyntaxAnalyzer.Analyze(tables.TokenTable);
                var rpnTokens = RPNTranslator.Analyze(tables, syntaxTree);
                DisplayRpnTokens(tables, rpnTokens);

                tables.IdentifiersTable.Display();
                tables.IntLiteralsTable.Display();
                tables.FloatLiteralsTable.Display();

                //Console.WriteLine("\nInterpreting:");
                //new RPNArithmeticInterpreter().Interpret(tokensTable, rpnTokens);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }

        private static void DisplayRpnTokens(TablesCollection tables, LinkedList<RPNToken> rpnTokens)
        {
            Console.WriteLine("RPN Tokens:");
            foreach (var rpnToken in rpnTokens)
            {
                Console.Write("{");
                if (rpnToken.Id.HasValue)
                {
                    if (Token.IsIdentifier(rpnToken.LexemeCode))
                    {
                        Console.Write($"\"{tables.IdentifiersTable[rpnToken.Id.Value].Name}\"");
                    }
                    else if (Token.IsIntLiteral(rpnToken.LexemeCode))
                    {
                        Console.Write($"\"{tables.IntLiteralsTable[rpnToken.Id.Value]}\"");
                    }
                    else if (Token.IsFloatLiteral(rpnToken.LexemeCode))
                    {
                        Console.Write($"\"{tables.FloatLiteralsTable[rpnToken.Id.Value]}\"");
                    }
                    else
                    {
                        Console.Write($"\"{rpnToken.Name}\"");
                    }
                    Console.Write($" ({rpnToken.Id.Value})");
                }
                else Console.Write(rpnToken.Name);
                Console.Write("}");
                Console.Write(" ");
            }
            Console.WriteLine();
        }
    }
}
