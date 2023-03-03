using GZip.Logic.Operations;
using GZip.Models;

namespace GZip
{
    /// <summary> Logic for file compression/decompression </summary>
    public  interface IGZipProcessor
    {
        /// <summary> GZip processing entry point </summary>
        /// <param name="args"> Console input </param>
        void Run(string[] args);

        /// <summary> Compress or decompress file </summary>
        /// <param name="pathParams"> Paths for input and output files </param>
        /// <param name="operationType"> Process operation type </param>
        void ProcessFile(FilePaths pathParams, OperationType operationType);
    }
}
