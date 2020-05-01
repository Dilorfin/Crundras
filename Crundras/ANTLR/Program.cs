using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Crundras;
using System;
using System.IO;

namespace ANTLR
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Crundras.exe %filename%");
                Console.ReadKey();
                return;
            }

            if (!File.Exists(args[0]))
            {
                Console.WriteLine($"File \"{args[0]}\" doesn't seem to exist.");
                Console.ReadKey();
                return;
            }

            try
            {
                var stream = new AntlrFileStream(args[0]);
                var lexer = new CrundrasLexer(stream);

                var tokenStream = new CommonTokenStream(lexer);

                var parser = new CrundrasParser(tokenStream);

                CrundrasListener listener = new CrundrasListener();

                var walker = new ParseTreeWalker();

                walker.Walk(listener, parser.program());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }
    }
}
