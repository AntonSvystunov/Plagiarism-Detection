using System;
using System.Collections.Generic;
using System.Text;

namespace Plagiarism.Data
{
    public interface IChunksLibrary
    {
        List<Chunk> GetLibrary();
    }
}
