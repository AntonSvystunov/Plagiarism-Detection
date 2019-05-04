using Plagiarism.Lexer;
using System;
using System.Collections.Generic;

namespace Plagiarism
{
    public static class Detector
    {
        static public void Detect(Chunk chunkA, Chunk chunkB, bool isCpp)
        {
            string linesA = String.Join('\n', chunkA._lines);
            string linesB = String.Join('\n', chunkB._lines);

            var lengthA = linesA.Length;
            var lengthB = linesB.Length;

            

            if (chunkA._lines.Count < 1000 && chunkB._lines.Count < 1000 && linesA.Length < 10000 && linesB.Length < 10000)
            {
                int dist = Chunk.LevenshteinDistance(linesA, linesB);
                Console.WriteLine("Levenshtein words distance is " + dist.ToString());

                var lineDist = Chunk.LevenshteinDistance(chunkA._lines.ToArray(), chunkB._lines.ToArray());
                Console.WriteLine("Levenshtein lines distance is " + lineDist.ToString());

                if (isCpp)
                {
                    var lexemDist = Chunk.LevenshteinDistanceLexeme(chunkA._lines.ToArray(), chunkB._lines.ToArray());
                    Console.WriteLine("Levenshtein lexeme distance is " + lexemDist.ToString());
                }
            }

            string longestCommon = Chunk.FindLongestCommonSubstring(linesA, linesB);
            Console.WriteLine("The longest common substring length is " + longestCommon.Length);

            if (isCpp)
            {
                var longestLexeme = Chunk.FindLongestCommonLexemeRow(linesA, linesB).Count;
                Console.WriteLine("The longest common lexeme row is " + longestLexeme.ToString());
            }

            //var res = ((longestCommon.Length/lengthA) * 100)/dist;
            Console.WriteLine();
        }

        public static void DetectAll(Chunk example, List<Chunk> chunks, bool isCpp)
        {
            Console.WriteLine("Distances for " + example.path);
            int cnt = 0;
            if (isCpp)
            {
                var lexer = new Plagiarism.Lexer.LexicalAnalyzer();
                var r = lexer.process(example.SourceCode);
                r.RemoveAll(l => l.type == LexerType.COMMENT);
                cnt = r.Count;
            }
            
            foreach (var c in chunks)
            {
                Console.WriteLine("Compare with " + c.path);
                if (isCpp)
                {
                    Console.WriteLine("Number of lexemes in example is " + cnt.ToString());
                }
                Detector.Detect(example, c, isCpp);
            }
        }
    }
}
