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
            Chunk chunk2 = Chunk.FromDirectory(@"E:\Projects C++\sandboxapp2", "*.cpp");

            LexicalAnalyzer lexical = new LexicalAnalyzer();
            var lex1 = lexical.process(chunk1.SourceCode);
            var lex2 = lexical.process(chunk2.SourceCode);

            foreach (var lex in lex1)
            {
                Console.WriteLine($"'{lex.value}' + {lex.type}");
            }

            Console.ReadLine();

        }
    }
}
