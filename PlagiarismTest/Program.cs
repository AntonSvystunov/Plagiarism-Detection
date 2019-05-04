﻿using Plagiarism;
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
            Chunk chunk1 = Chunk.FromDirectory(@"E:\Projects C++\sandboxapp", "*.cpp");
            Chunk chunk2 = Chunk.FromDirectory(@"E:\Projects C++\sandboxapp2", "*.cpp");

            var l = Chunk.FindLongestCommonLexemeRow(chunk1.SourceCode, chunk2.SourceCode);
            Console.WriteLine(l.Count);
            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 1)
                {
                    Console.WriteLine("Too few args");
                    Console.WriteLine("USAGE <path-to-project> [<filter>] [-cpp]");
                }

                string filename = args[0];
                string filter = null;
                bool isCpp = false;

                if (args.Length > 1)
                {

                }

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
