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

        static void Main(string[] args)
        {
            //Chunk chunk1 = Chunk.FromDirectory(@"E:\Projects C++\sandboxapp", "*.cpp");
            //Chunk chunk2 = Chunk.FromDirectory(@"E:\Projects C++\sandboxapp2", "*.cpp");

            //LexicalAnalyzer lexical = new LexicalAnalyzer();
            ////var lex1 = lexical.process(chunk1.SourceCode);
            ////var lex2 = lexical.process(chunk2.SourceCode);

            //foreach (var lex in lex1)
            //{
            //    Console.WriteLine($"'{lex.value}' + {lex.type}");
            //}

            //Chunk chunk = new Chunk();

            //if(args.Length < 1)
            //{
            //    Console.WriteLine($"'{lex.value}' - {lex.type}");
            //}

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
            Console.ReadKey();

            var example = Chunk.FromDirectory("C:/Alex/C++/lab22", "*.cpp");

            Console.WriteLine("Distances for " + example.path);
            GetAllDistances(example, chunks);

            //foreach (var c in chunks)
            //{
            //    Console.WriteLine(c.path);
            //    foreach(var line in c._lines)
            //    {
            //        Console.WriteLine(line);
            //    }
            //    Console.WriteLine();
            //}

        }

        static void GetAllDistances(Chunk chunk, List<Chunk> library)
        {
            foreach(var c in library)
            {
                //int dist = Chunk.LevenshteinDistance(String.Join('\n', c._lines), String.Join('\n', chunk._lines));
                //Console.WriteLine(c.path + " --- " + dist.ToString());
                Console.WriteLine("Comparing with: " + c.path);
                Detector.Detect(chunk, c);
            }
        }
    }
}
