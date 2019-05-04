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
                if (args.Length < 3)
                {
                    Console.WriteLine("Too few args");
                    Console.WriteLine("USAGE <path-to-project> <path-to-codebase> <filter> [-cpp]");
                }

                string filename = args[0];
                string libbase = args[1];
                string filter = args[2];
                bool isCpp = false;

                if (args.Length > 3)
                {
                    isCpp = args[3].Trim() == "-cpp";
                }

                FSChunkLibrary library;

                try
                {
                    library = new FSChunkLibrary(libbase, filter);
                }
                catch
                {
                    Console.WriteLine("Errors");
                    return;
                }

                var chunks = library.GetLibrary();

                var example = Chunk.FromDirectory(filename, filter);

                Detector.DetectAll(example, chunks, isCpp);

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
