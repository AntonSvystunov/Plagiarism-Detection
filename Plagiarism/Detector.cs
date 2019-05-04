using Plagiarism.Lexer;
using System;
using System.Collections.Generic;

namespace Plagiarism
{
    public static class Detector
    {
        static public void Detect(Chunk chunkA, Chunk chunkB)
        {
            string linesA = String.Join('\n', chunkA._lines);
            string linesB = String.Join('\n', chunkB._lines);

            int dist = Chunk.LevenshteinDistance(linesA, linesB);
            Console.WriteLine("Levenshtein distance is " + dist.ToString());

            string longestCommon = Chunk.FindLongestCommonSubstring(linesA, linesB);
            Console.WriteLine("The longest common substring length is " + longestCommon.Length);

            var num = Chunk.FindLongestCommonLexemeRow(linesA, linesB).Count;
            Console.WriteLine("The longest common lexeme row is " + num.ToString());

            Console.WriteLine();
        }

        public static void DetectAll(Chunk example, List<Chunk> chunks)
        {
            Console.WriteLine("Distances for " + example.path);

            var lexer = new Plagiarism.Lexer.LexicalAnalyzer();
            var r = lexer.process(example.SourceCode);
            r.RemoveAll(l => l.type == LexerType.COMMENT);
            var cnt = r.Count;
            foreach (var c in chunks)
            {
                Console.WriteLine("Compare with " + c.path);
                Console.WriteLine("Number of lexemes in example is " + cnt.ToString());
                Detector.Detect(example, c);
            }
        }
    }
}
