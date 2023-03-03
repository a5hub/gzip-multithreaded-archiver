namespace GZip.Logic.Logic.TaskManagement;

/// <summary> Generic task process logic for both compression and decompression parts </summary>
public interface ITaskProcessLogic
{
    /// <summary> Process one task </summary>
    /// <param name="task"> Compression or decompression gzip task </param>
    void ProcessTask(GZipTask task);
}