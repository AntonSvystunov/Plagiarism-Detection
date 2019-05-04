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

            if(args.Length < 1)
            {
                Console.WriteLine("Wrong path");
                return;
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

            var chunks = library.chunks;

        }
    }
}
