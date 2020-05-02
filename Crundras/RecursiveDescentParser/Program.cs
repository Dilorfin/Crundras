using Crundras.Common;
using Crundras.LexicalAnalyzer;
using System;
using System.IO;

namespace RecursiveDescentParser
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
                var tokensTable = LexicalAnalyzer.AnalyzeFile(args[0]);

                var syntaxTree = new RecursiveDescentParser(tokensTable).Analyze();
                PrintSyntaxTree(tokensTable, syntaxTree);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }

        private static void PrintSyntaxTree(TokenTable tokensTable, SyntaxTreeNode node, int level = 0)
        {
            if (node == null)
            {
                return;
            }

            if (node.LexemeCode != uint.MaxValue)
            {
                Console.Write($"{new string('-', level)} {TokenTable.GetLexemeName(node.LexemeCode)} ");
                if (node.Id.HasValue)
                {
                    Console.Write(node.LexemeCode == TokenTable.GetLexemeId("identifier")
                        ? $"\"{tokensTable.IdentifiersTable[node.Id.Value]}\""
                        : $"\"{tokensTable.LiteralsTable[node.Id.Value]}\"");
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
                PrintSyntaxTree(tokensTable, child, level + 1);
            }
        }
    }
}
