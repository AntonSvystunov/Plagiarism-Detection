using Plagiarism.Lexer;
using System;
using System.Collections.Generic;

namespace Plagiarism
{
    public static class Detector
    {
        static public void Detect(Chunk chunkA, Chunk chunkB, bool isCpp,int lexlenA = 0)
        {
            string linesA = String.Join('\n', chunkA._lines);
            string linesB = String.Join('\n', chunkB._lines);

            var lengthA = linesA.Length;
            var lengthB = linesB.Length;

            int longestLexeme = 0;
            int dist = -1, lineDist = -1;
            double lexemDist =-1;


            bool doleven = chunkA._lines.Count < 1000 && chunkB._lines.Count < 1000 && linesA.Length < 10000 && linesB.Length < 10000;
            
            //if (doleven)
            //{
                dist = Chunk.LevenshteinDistance(linesA, linesB);
                Console.WriteLine("Levenshtein words distance is " + dist.ToString());

                lineDist = Chunk.LevenshteinDistance(chunkA._lines.ToArray(), chunkB._lines.ToArray());
                Console.WriteLine("Levenshtein lines distance is " + lineDist.ToString());

                if (isCpp)
                {
                    lexemDist = Chunk.LevenshteinDistanceLexeme(chunkA._lines.ToArray(), chunkB._lines.ToArray());
                    Console.WriteLine("Levenshtein lexeme distance is " + lexemDist.ToString());
                }
            //}

            string longestCommon = Chunk.FindLongestCommonSubstring(linesA, linesB);
            Console.WriteLine("The longest common substring length is " + longestCommon.Length);

            if (isCpp)
            {
                longestLexeme = Chunk.FindLongestCommonLexemeRow(linesA, linesB).Count;
                Console.WriteLine("The longest common lexeme row is " + longestLexeme.ToString());
            }

            double res = 0;

            if (longestCommon.Length / linesA.Length > 0.01)
            {
                res += longestCommon.Length / linesA.Length;
            }

            if (isCpp&&lexlenA>0)
            {
                if (longestLexeme / linesA.Length > 0.01)
                {
                    res += longestCommon.Length / lexlenA;
                }
            }

            //if (doleven)
            //{
                if (dist != -1)
                {
                    if (dist != 0)
                    {
                        res += 300 / dist;
                    }
                    else
                    {
                        res += 20000;
                    }
                }

                if (lineDist != -1)
                {
                    if (lineDist != 0)
                    {
                        res += 5 / lineDist;
                    }
                    else
                    {
                        res += 20000;
                    }
                }

                if (isCpp)
                {
                    if (lexemDist != -1)
                    {
                        if (lexemDist != 0)
                        {
                            res += 5 / lexemDist;
                        }
                        else
                        {
                            res += 20000;
                        }
                    }
                }

            //}

            double tanhres = Math.Tanh(res);
            double score = Math.Round(tanhres * 100, 2);


            Console.WriteLine("Plagiarism score: " + score + "%");

            if(score > 0.9)
            {
                Console.WriteLine("NICE TRY, COPYCAT!");
            }

            Console.WriteLine();
        }

        public static void DetectAll(Chunk example, List<Chunk> chunks, bool isCpp)
        {
            Console.WriteLine("Distances for " + example.path);
            Console.WriteLine();

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
                Console.WriteLine("Comparing with " + c.path);
                if (isCpp)
                {
                    Console.WriteLine("Number of lexemes in example is " + cnt.ToString());
                }
                Detector.Detect(example, c, isCpp,cnt);
            }
        }
    }
}
