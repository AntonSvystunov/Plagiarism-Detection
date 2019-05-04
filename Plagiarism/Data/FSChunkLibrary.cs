using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Plagiarism.Data
{
    public class FSChunkLibrary : IChunksLibrary
    {
        public List<Chunk> chunks;

        public FSChunkLibrary(string dirPath)
        {
            chunks = new List<Chunk>();

            var dirs = Directory.GetDirectories(dirPath).ToList();

            foreach(var dir in dirs)
            {
                chunks.Add(Chunk.FromDirectory(dir, ".cpp"));
            }
        }

        public List<Chunk> GetLibrary()
        {
            throw new NotImplementedException();
        }
    }
}
