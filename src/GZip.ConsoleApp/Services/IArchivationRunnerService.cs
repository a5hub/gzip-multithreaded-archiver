namespace GZip.ConsoleApp.Services;

/// <summary> Logic for file compression/decompression </summary>
public  interface IArchivationRunnerService
{
    /// <summary> GZip processing entry point </summary>
    /// <param name="args"> Console input </param>
    void Run(string[] args);
}