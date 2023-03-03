using FileProcessor.Common;
using FileProcessor.Configuration;
using FileProcessor.Logic.Archivation;
using FileProcessor.Models;
using System.Collections.Generic;
using System.IO.Abstractions;

namespace FileProcessor.Logic.Operations
{
    /// <summary> Implements compression logic methods </summary>
    public class CompressionOperation : IOperation
    {
        /// <summary> Application configuration  </summary>
        private readonly IAppConfig _appConfig;

        /// <summary> Logic provides methods to compress and decompress data </summary>
        private readonly IArchiveProvider _archiveProvider;

        /// <summary> File system access abstraction </summary>
        private readonly IFileSystem _fileSystem;

        /// <summary> Implements compression logic methods </summary>
        public CompressionOperation(IAppConfig appConfig,
            IArchiveProvider archiveProvider,
            IFileSystem fileSystem)
        {
            _appConfig = appConfig;
            _archiveProvider = archiveProvider;
            _fileSystem = fileSystem;
        }

        /// <summary> Splits file to separated chunks </summary>
        /// <param name="filePath"> Source file path </param>
        /// <returns> Collection of chunks </returns>
        public IEnumerable<FileChunk> SplitFile(string filePath)
        {
            Guard.NotNull(filePath, $"{nameof(filePath)}");

            var chunks = new List<FileChunk>();
            var fileLength = _fileSystem.FileInfo.FromFileName(filePath).Length;
            var bytesAvailable = fileLength;

            while (bytesAvailable > 0)
            {
                var readBytes = bytesAvailable < _appConfig.CompressionChunkSize
                    ? bytesAvailable
                    : _appConfig.CompressionChunkSize;

                chunks.Add(new FileChunk
                {
                    StartPosition = fileLength - bytesAvailable,
                    ReadBytes = (int)readBytes
                });

                bytesAvailable -= readBytes;
            }

            return chunks;
        }

        /// <summary> Process separated chunk </summary>
        /// <param name="chunk"> Input chunk to process</param>
        /// <returns> Processed chunk </returns>
        public byte[] ProcessChunk(byte[] chunk)
        {
            Guard.NotNull(chunk, $"{nameof(chunk)}");

            return _archiveProvider.Compress(chunk);
        }
    }
}
