using FileProcessor.Models;
using System.Collections.Generic;

namespace FileProcessor.Logic.Operations
{
    /// <summary> Operation specific logic </summary>
    public interface IOperation
    {
        /// <summary> Splits file to separated chunks </summary>
        /// <param name="filePath"> Source file path </param>
        /// <returns> Collection of chunks </returns>
        IEnumerable<FileChunk> SplitFile(string filePath);

        /// <summary> Process separated chunk </summary>
        /// <param name="chunk"> Input chunk to process</param>
        /// <returns> Processed chunk </returns>
        byte[] ProcessChunk(byte[] chunk);
    }
}
