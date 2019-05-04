using Plagiarism;
using Plagiarism.Lexer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PlagiarismTest
{
    class Program
    {

        static void Main(string[] args)
        {
            //Chunk chunk = Chunk.FromDirectory(@"E:\JavaScriptProjects\empty-example\","*.js");
            var path =  @"E:\JavaScriptProjects\Java\LabSysProg3\input.txt";
            var lexer = new LexicalAnalyzer(File.OpenText(path));
            lexer.process();
            Console.ReadLine();
        }
    }
}
