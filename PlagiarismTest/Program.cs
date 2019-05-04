using Plagiarism;
using System;
using System.Collections.Generic;
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

            if(args[0].Length < 1)
            {
                Console.WriteLine("Wrong path");
                return;
            }

            try
            {
                FSChunkLibrary library = new FSChunkLibrary(args[0]);
            }
            catch
            {
                Console.WriteLine("Errors");
                return;
            }

        }
    }
}
