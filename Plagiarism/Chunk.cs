using System;
using System.Collections.Generic;
using System.Text;

namespace Plagiarism
{
    public class Chunk
    {
        private string[] _lines;

        public Chunk(string[] lines)
        {
            _lines = lines;
        }

        public double GetDistance(Chunk other)
        {
            return 0;
        }
    }
}
