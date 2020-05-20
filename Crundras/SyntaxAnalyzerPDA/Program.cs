using Crundras.Common;
using Crundras.Common.Tables;
using Crundras.LexicalAnalyzer;
using System;
using System.IO;

namespace SyntaxAnalyzerPDA
{
    internal class Program
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
                PrintSyntaxTree(tables, syntaxTree);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }

        private static void PrintSyntaxTree(TablesCollection tables, SyntaxTreeNode node, int level = 0)
        {
            if (node == null)
            {
                return;
            }

            if (node.LexemeCode != uint.MaxValue)
            {
                Console.Write($"{new string('-', level)} {LexemesTable.GetLexemeName(node.LexemeCode)} ");
                if (node.Id.HasValue)
                {
                    if (Token.IsIdentifier(node.LexemeCode))
                    {
                        Console.Write($"\"{tables.IdentifiersTable[node.Id.Value].Name}\"");
                    }
                    else if (Token.IsIntLiteral(node.LexemeCode))
                    {
                        Console.Write($"\"{tables.IntLiteralsTable[node.Id.Value]}\"");
                    }
                    else if (Token.IsFloatLiteral(node.LexemeCode))
                    {
                        Console.Write($"\"{tables.FloatLiteralsTable[node.Id.Value]}\"");
                    }
                    Console.Write($" ({node.Id.Value})");
                }
                Console.WriteLine();
            }

            if (node.Children == null)
            {
                return;
            }

            foreach (var child in node.Children)
            {
                PrintSyntaxTree(tables, child, level + 1);
            }
        }
    }
}
