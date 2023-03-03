using System.IO.Abstractions;
using GZip.Common.Common;
using GZip.Logic.Configuration;
using GZip.Logic.Logic.Archivation;
using GZip.Logic.Models;

namespace GZip.Logic.Logic.Operations;

/// <summary> Implements compression logic methods </summary>
public class DecompressionOperation : IOperation
{
    /// <summary> Application configuration  </summary>
    private readonly IAppConfig _appConfig;

    /// <summary> Logic provides methods to compress and decompress data </summary>
    private readonly IArchiveProvider _archiveProvider;

    /// <summary> File system access abstraction </summary>
    private readonly IFileSystem _fileSystem;

    /// <summary> Implements compression logic methods </summary>
    public DecompressionOperation(IAppConfig appConfig,
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
        var bufferSize = _appConfig.CompressionChunkSize;

        using var fs = _fileSystem.FileStream.Create(filePath, FileMode.Open, System.IO.FileAccess.Read);

        byte[] gzipHeader = {0x1f, 0x8b, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00};

        var fileLength = _fileSystem.FileInfo.FromFileName(filePath).Length;
        var bytesAvailable = fileLength;
        var chunkIndex = 0;

        while (bytesAvailable > 0)
        {
            var gzipBlock = new List<byte>(bufferSize);

            if (chunkIndex == 0)
            {
                fs.Read(gzipHeader, 0, gzipHeader.Length);
                bytesAvailable -= gzipHeader.Length;
            }

            gzipBlock.AddRange(gzipHeader);

            var gzipHeaderMatchsCount = 0;
            while (bytesAvailable > 0)
            {
                var readByte = fs.ReadByte();
                var curByte = BitConverter.GetBytes(readByte);
                gzipBlock.Add(curByte[0]);
                bytesAvailable--;

                if (curByte[0] == gzipHeader[gzipHeaderMatchsCount])
                {
                    gzipHeaderMatchsCount++;
                    if (gzipHeaderMatchsCount != gzipHeader.Length)
                        continue;

                    gzipBlock.RemoveRange(gzipBlock.Count - gzipHeader.Length,
                        gzipHeader.Length);
                    break;
                }

                gzipHeaderMatchsCount = 0;
            }

            var gzipBlockStartPosition = 0L;
            var gzipBlockLength = gzipBlock.ToArray().Length;
            if (chunkIndex > 0)
            {
                gzipBlockStartPosition = fileLength - bytesAvailable - gzipHeader.Length - gzipBlockLength;
                if (gzipBlockStartPosition + gzipHeader.Length + gzipBlockLength == fileLength)
                {
                    gzipBlockStartPosition += gzipHeader.Length;
                }
            }

            chunks.Add(new FileChunk
            {
                StartPosition = gzipBlockStartPosition,
                ReadBytes = gzipBlockLength
            });

            chunkIndex++;
        }

        return chunks;
    }

    /// <summary> Process separated chunk </summary>
    /// <param name="chunk"> Input chunk to process</param>
    /// <returns> Processed chunk </returns>
    public byte[] ProcessChunk(byte[] chunk)
    {
        Guard.NotNull(chunk, $"{nameof(chunk)}");

        return _archiveProvider.Decompress(chunk);
    }
}