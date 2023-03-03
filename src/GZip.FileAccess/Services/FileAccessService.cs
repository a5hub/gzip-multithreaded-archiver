using System.IO.Abstractions;
using GZip.Common.Common;

namespace GZip.FileAccess.Services;

/// <summary> Manage access to physical files </summary>
public class FileAccessService : IFileAccessService
{
    /// <summary> File system access abstraction </summary>
    private readonly IFileSystem _fileSystem;

    /// <summary> Manage access to physical files </summary>
    public FileAccessService(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }

    /// <summary> Read chunk from file </summary>
    /// <param name="filePath"> Path to source file</param>
    /// <param name="startPosition"> Reading starts from byte </param>
    /// <param name="chunkLength"> Amount of bytes to read </param>
    /// <returns> Array of bites being read </returns>
    public byte[] ReadFileChunk(string filePath, long startPosition, int chunkLength)
    {
        Guard.NotNull(filePath, $"{nameof(filePath)}");

        var result = new byte[chunkLength];

        using var stream = _fileSystem.FileStream.Create(filePath, FileMode.Open, 
            System.IO.FileAccess.Read, FileShare.Read);

        stream.Seek(startPosition, SeekOrigin.Begin);
        stream.Read(result, 0, chunkLength);

        return result;
    }

    /// <summary> Write chunk to file </summary>
    /// <param name="filePath"> Path to source file</param>
    /// <param name="savedData"> Data to be saved to file </param>
    public void WriteFileChunk(string filePath, byte[] savedData)
    {
        Guard.NotNull(filePath, $"{nameof(filePath)}");

        using var stream = _fileSystem.FileStream.Create(filePath, FileMode.OpenOrCreate, 
            System.IO.FileAccess.Write, FileShare.None);

        stream.Seek(0, SeekOrigin.End);
        stream.Write(savedData);
    }
}