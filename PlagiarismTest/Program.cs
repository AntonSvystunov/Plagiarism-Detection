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
            //Chunk chunk = Chunk.FromDirectory(@"E:\JavaScriptProjects\empty-example\","*.js");

            Chunk chunk = new Chunk();

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

            var chunks = library.chunks;

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
    }
}
