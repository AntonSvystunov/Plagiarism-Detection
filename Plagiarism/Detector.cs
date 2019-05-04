using System;

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
            Console.WriteLine();
        }
    }
}
