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
            Chunk chunk1 = Chunk.FromDirectory(@"E:\Projects C++\sandboxapp", "*.cpp");
            Chunk chunk2 = Chunk.FromDirectory(@"E:\Projects C++\sandboxapp", "*.cpp");

            Console.WriteLine(chunk1.GetDistance(chunk2));
            Console.ReadLine();

        }
    }
}
