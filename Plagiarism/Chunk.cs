using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace Plagiarism
{
    public class Chunk
    {
        private List<string> _lines;

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
            return 0;
        }

        public static Chunk FromFile(string filename)
        {
            var l = File.ReadAllLines(filename);
            return new Chunk(l);
        }

        public static Chunk FromDirectory(string dirPath, string filter)
        {
            filter = string.Format("^{0}$", filter.Replace("*", ".*"));

            var files = Directory.EnumerateFiles(dirPath);
            files = files.Where(x => Regex.IsMatch(x, filter, RegexOptions.IgnoreCase) == true).ToList();

            var res = new Chunk();
            foreach (var file in files)
            {
                res.MergeChunks(Chunk.FromFile(file));
            }

            return res;
        }

        public Chunk MergeChunks(Chunk chunkToAdd)
        {
            _lines.AddRange(chunkToAdd._lines);
            return this;
        }
    }
}
