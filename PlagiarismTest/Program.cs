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
            var code = @"for (int i = 0; i < 5; i++) res *= i";
            LexicalAnalyzer lexer = new LexicalAnalyzer();
            var lexemes = lexer.process(code);
            foreach(var lex in lexemes)
            {
                Console.WriteLine($"'{lex.value}' - {lex.type}");
            }
            Console.ReadLine();
        }
    }
}
