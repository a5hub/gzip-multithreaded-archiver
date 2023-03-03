using GZip.Logic.Logic.Operations;
using GZip.Logic.Models;

namespace GZip.ConsoleApp.Adapter.Services;

public interface IArchiveProcessorService
{
    /// <summary> Compress or decompress file </summary>
    /// <param name="pathParams"> Paths for input and output files </param>
    /// <param name="operationType"> Process operation type </param>
    Task DoArchivation(FilePaths pathParams, OperationType operationType);
}