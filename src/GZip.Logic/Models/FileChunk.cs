namespace GZip.Logic.Models;

/// <summary> Contains parameters needed during compression logic </summary>
public class FileChunk
{
    /// <summary> Position where start to read from begin of file </summary>
    public long StartPosition { get; set; }

    /// <summary> Amount of bites to read </summary>
    public int ReadBytes { get; set; }
}