using LexicalAnalyzer;
using System;
using System.IO;

namespace SyntaxAnalyzerPDA
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

                var syntaxTree = new SyntaxAnalyzer(tokensTable).Analyze();
                PrintSyntaxTree(tokensTable, syntaxTree);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }

        static void PrintSyntaxTree(TokenTable tokensTable, SyntaxTreeNode node, int level = 0)
        {
            if (node == null) return;

            Console.Write($"{new string('-', level)} {node.Name} ");
            if (node.Id != 0)
            {
                ////Console.Write(node.Name == "identifier"
                //    ? $"\"{tokensTable.IdentifiersTable[node.Id]}\""
                //    : $"\"{tokensTable.LiteralsTable[node.Id]}\"");
                Console.Write($" ({node.Id})");
            }
            Console.WriteLine();

            if (node.GetChildren() == null)
                return;

            foreach (var child in node.GetChildren())
            {
                PrintSyntaxTree(tokensTable, child, level + 1);
            }
        }
    }
}
