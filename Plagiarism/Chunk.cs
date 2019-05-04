using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using Plagiarism.Lexer;

namespace Plagiarism
{
    public class Chunk
    {
        public List<string> _lines { get; }

        public string SourceCode => string.Join('\n', _lines);

        public string path;

        public Chunk()
        {
            _lines = new List<string>();
        }

        public Chunk(string[] lines)
        {
            _lines = new List<string>(lines);
        }

        public double GetDistance(Chunk other)
        {
            double res = 0;
            foreach (string lineFromThisChunk in _lines)
            {
                int min = 99999;
                foreach (string lineFromAnotherChunk in other._lines)
                {
                    if (LevenshteinDistance(lineFromThisChunk, lineFromAnotherChunk) < min)
                        min = LevenshteinDistance(lineFromThisChunk, lineFromAnotherChunk);
                }
                res += min;
            }
            return res;
        }

        
        public static Chunk FromFile(string filename)
        {
            var l = File.ReadAllLines(filename);
            List<string> resLines = new List<string>();

            foreach (var line in l)
            {
                if (line.Trim().Length > 0)
                {
                    resLines.Add(line);
                }
            }

            return new Chunk(resLines.ToArray());
        }

        public static Chunk FromDirectory(string dirPath, string filter)
        {
            filter = string.Format("^{0}$", filter.Replace("*", ".*"));

            var files = Directory.GetFiles(dirPath).ToList();
            files = files.Where(x => Regex.IsMatch(x, filter, RegexOptions.IgnoreCase) == true).ToList();

            var res = new Chunk();
            foreach (var file in files)
            {
                res.MergeChunks(Chunk.FromFile(file));
            }

            res.path = dirPath;

            return res;
        }

        public Chunk MergeChunks(Chunk chunkToAdd)
        {
            _lines.AddRange(chunkToAdd._lines);
            return this;
        }

        public static int LevenshteinDistance(string str1, string str2)
        {
            var n = str1.Length;
            var m = str2.Length;
            int[,] distanceMatrix = new int[n + 1, m + 1];

            if (n == 0) return m;
            if (m == 0) return n;

            for (int i = 0; i <= n; distanceMatrix[i, 0] = i++) ;
            for (int i = 0; i <= m; distanceMatrix[0, i] = i++) ;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int distance = (str1[i - 1] == str2[j - 1]) ? 0 : 1;

                    distanceMatrix[i, j] = Math.Min(Math.Min(distanceMatrix[i - 1, j] + 1,
                        distanceMatrix[i, j - 1] + 1), distanceMatrix[i - 1, j - 1] + distance);
                }
            }

            return distanceMatrix[n, m];
        }

        public static int LevenshteinDistance(string[] str1, string[] str2)
        {
            int n = str1.Length;
            int m = str2.Length;
            int[,] distanceMatrix = new int[n + 1, m + 1];

            if (n == 0) return m;
            if (m == 0) return n;

            for (int i = 0; i <= n; distanceMatrix[i, 0] = i++) ;
            for (int i = 0; i <= m; distanceMatrix[0, i] = i++) ;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int distance = (str1[i - 1] == str2[j - 1]) ? 0 : 1;

                    distanceMatrix[i, j] = Math.Min(
                        Math.Min(distanceMatrix[i - 1, j] + 1, distanceMatrix[i, j - 1] + 1),
                        distanceMatrix[i - 1, j - 1] + distance);
                }
            }

            return distanceMatrix[n, m];
        }

        public static double LevenshteinDistanceLexeme(string[] str1, string[] str2)
        {
            int n = str1.Length;
            int m = str2.Length;
            double[,] distanceMatrix = new double[n + 1, m + 1];

            if (n == 0) return m;
            if (m == 0) return n;

            for (int i = 0; i <= n; distanceMatrix[i, 0] = i++) ;
            for (int i = 0; i <= m; distanceMatrix[0, i] = i++) ;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    double distance = FindLongestCommonLexemeRowCoef(str1[i - 1], str2[j - 1]);

                    distanceMatrix[i, j] = Math.Min(
                        (double)Math.Min(distanceMatrix[i - 1, j] + 1, distanceMatrix[i, j - 1] + 1),
                        distanceMatrix[i - 1, j - 1] + distance);
                }
            }

            return distanceMatrix[n, m];
        }

        public static double FindLongestCommonLexemeRowCoef(string str1, string str2)
        {
            var lexer = new LexicalAnalyzer();

            List<Lexeme> lex1 = lexer.process(str1);

            lexer = new LexicalAnalyzer();
            List<Lexeme> lex2 = lexer.process(str2);

            lex1.RemoveAll(lex => lex.type == LexerType.COMMENT);
            lex2.RemoveAll(lex => lex.type == LexerType.COMMENT);

            int[,] a = new int[lex1.Count + 1, lex2.Count + 1];
            int row = 0;
            int col = 0;

            for (var i = 0; i < lex1.Count; i++)
            {
                for (var j = 0; j < lex2.Count; j++)
                {
                    if (lex1[i].type == lex2[j].type)
                    {
                        int len = a[i + 1, j + 1] = a[i, j] + 1;
                        if (len > a[row, col])
                        {
                            row = i + 1;
                            col = j + 1;
                        }
                    }
                }
            }

            if(lex1.Count == 0)
            {
                return 0;
            }
            return lex1.GetRange(row - a[row, col], a[row, col]).Count / lex1.Count;
        }

        public static List<Lexeme> FindLongestCommonLexemeRow(string str1, string str2)
        {
            var lexer = new LexicalAnalyzer();

            List<Lexeme> lex1 = lexer.process(str1);

            lexer = new LexicalAnalyzer();
            List<Lexeme> lex2 = lexer.process(str2);

            lex1.RemoveAll(lex => lex.type == LexerType.COMMENT);
            lex2.RemoveAll(lex => lex.type == LexerType.COMMENT);

            int[,] a = new int[lex1.Count + 1, lex2.Count + 1];
            int row = 0;
            int col = 0;

            for (var i = 0; i < lex1.Count; i++)
            {
                for (var j = 0; j < lex2.Count; j++)
                {
                    if (lex1[i].type == lex2[j].type)
                    {
                        int len = a[i + 1, j + 1] = a[i, j] + 1;
                        if (len > a[row, col])
                        {
                            row = i + 1;
                            col = j + 1;
                        }
                    }
                }
            }

            return lex1.GetRange(row - a[row, col], a[row, col]);
        }

        public static string FindLongestCommonSubstring(string str1, string str2)
        {
            int[,] a = new int[str1.Length + 1, str2.Length + 1];
            int row = 0;
            int col = 0;

            for (var i = 0; i < str1.Length; i++)
            {
                for (var j = 0; j < str2.Length; j++)
                {
                    if (str1[i] == str2[j])
                    {
                        int len = a[i + 1, j + 1] = a[i, j] + 1;
                        if (len > a[row, col])
                        {
                            row = i + 1;
                            col = j + 1;
                        }
                    }
                }
            }

            return str1.Substring(row - a[row, col], a[row, col]);
        }
    }
}
