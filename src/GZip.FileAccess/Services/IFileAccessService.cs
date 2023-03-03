namespace GZip.FileAccess.Services;

/// <summary> Manage access to physical files </summary>
public interface IFileAccessService
{
    /// <summary> Read chunk from file </summary>
    /// <param name="filePath"> Path to source file</param>
    /// <param name="startPosition"> Reading starts from byte </param>
    /// <param name="chunkLength"> Amount of bytes to read </param>
    /// <returns> Array of bites being read </returns>
    byte[] ReadFileChunk(string filePath, long startPosition, int chunkLength);

    /// <summary> Write chunk to file </summary>
    /// <param name="filePath"> Path to source file</param>
    /// <param name="savedData"> Data to be saved to file </param>
    void WriteFileChunk(string filePath, byte[] savedData);
}