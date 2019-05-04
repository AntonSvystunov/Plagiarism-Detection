using Plagiarism;
using Plagiarism.Lexer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Plagiarism.Data;

namespace PlagiarismTest
{
    class Program
    {
        static void Process(string filename)
        {

        }

        static void Test1()
        {
            //Chunk chunk1 = Chunk.FromDirectory(@"E:\Projects C++\sandboxapp", "*.cpp");
            //Chunk chunk2 = Chunk.FromDirectory(@"E:\Projects C++\sandboxapp2", "*.cpp");

            //var l = Chunk.FindLongestCommonLexemeRow(chunk1.SourceCode, chunk2.SourceCode);
            //Console.WriteLine(l.Count);
            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            //Chunk chunk1 = Chunk.FromDirectory(@"E:\Projects C++\sandboxapp", "*.cpp");
            //Chunk chunk2 = Chunk.FromDirectory(@"E:\Projects C++\sandboxapp", "*.cpp");
            try
            {
                if (args.Length < 1)
                {
                    Console.WriteLine("Too few args");
                    Console.WriteLine("USAGE <path-to-project> [<filter>] [-cpp]");

                string filename = args[0];
                string filter = null;
                bool isCpp = false;

                if (args.Length > 1)
                {

                }

                FSChunkLibrary library;

                try
                {
                    library = new FSChunkLibrary(args[0]);
                }
                catch
                {
                    Console.WriteLine("Errors");
                    return;
                }

                var chunks = library.GetLibrary();

            Console.WriteLine(Chunk.FindLongestCommonSubstring("britanicaeng", "britanicahin"));
            Console.WriteLine(Chunk.FindLongestCommonSubstring("britanicaengqwerqwerqwerqwer", "britanicahinqwerqwerqwerqwer"));
            Console.WriteLine(Chunk.FindLongestCommonSubstring("britani1234567890caeng", "britanici1234567890ahin"));

            string[] str1 = { "qwer1", "qwer2", "qwer5" };
            string[] str2 = { "qwer1", "qwer2" };
            Console.WriteLine(Chunk.LevenshteinDistance(str1, str2));
            Console.ReadKey();

                var example = Chunk.FromDirectory("C:/Alex/C++/lab22", "*.cpp");

                Console.WriteLine("Distances for " + example.path);
                GetAllDistances(example, chunks);

                ////foreach (var c in chunks)
                ////{
                ////    Console.WriteLine(c.path);
                ////    foreach(var line in c._lines)
                ////    {
                ////        Console.WriteLine(line);
                ////    }
                ////    Console.WriteLine();
                ////}

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        private static void GetAllDistances(Chunk example, List<Chunk> chunks)
        {
            var lexer = new Plagiarism.Lexer.LexicalAnalyzer();
            var r = lexer.process(example.SourceCode);
            r.RemoveAll(l => l.type == LexerType.COMMENT);
            var cnt = r.Count;
            foreach (var c in chunks)
            {
                Console.WriteLine("Compare with " + c.path);
                Console.WriteLine("Number of lexemes in example is " + cnt.ToString());
                Detector.Detect(example, c);
            }
        }
    }
}
