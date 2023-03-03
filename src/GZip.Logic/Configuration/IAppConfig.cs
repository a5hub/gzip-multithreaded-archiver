namespace GZip.Logic.Configuration;

/// <summary> Application configuration </summary>
public interface IAppConfig
{
    /// <summary> If manual processors count chosen, gets its value,
    /// but not more than environment logical processors count </summary>
    int ProcessorsCount { get; }

    /// <summary> Size of compression chunk </summary>
    int CompressionChunkSize { get; }
}