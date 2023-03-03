using System;
using System.Collections.Generic;
using GZip.Common.Common;
using GZip.ConsoleApp.Adapter.Services;
using GZip.Logic.Logic.Operations;
using GZip.Logic.Models;
using Serilog;

namespace GZip.ConsoleApp.Services;

/// <summary> Logic for file compression/decompression </summary>
public class ArchivationRunnerService : IArchivationRunnerService
{
    private readonly Dictionary<string, OperationType> _operations = new()
    {
        {"compress", OperationType.Compress},
        {"decompress", OperationType.Decompress}
    };

    private readonly IArchiveProcessorService _archiveProcessorService;

    /// <summary> Logic for file compression/decompression </summary>
    public ArchivationRunnerService(IArchiveProcessorService archiveProcessorService)
    {
        _archiveProcessorService = archiveProcessorService;
    }

    /// <summary> GZip processing entry point </summary>
    /// <param name="args"> Console input </param>
    public void Run(string[] args)
    {
        Guard.NotNull(args, $"{nameof(args)}");
        Guard.True(args.Length > 0, "Input parameters is empty.");

        // Check input and prepare initial params for processing start
        if (!_operations.TryGetValue(args[0], out var operationType))
        {
            Console.WriteLine("Wrong operation type, please input exactly compress/decompress");
            return;
        }

        var pathParams = new FilePaths
        {
            InputFilePath = args[1],
            OutputFilePath = args[2]
        };

        Console.WriteLine("{0} Process started, please wait", DateTime.Now);

        Log.Information(
            $"File processing started input file {pathParams.InputFilePath}, output file {pathParams.OutputFilePath}, operation {args[0]}");

        // Execute processing
        _archiveProcessorService.DoArchivation(pathParams, operationType);

        Log.Information("File processing finished.");
        Console.WriteLine("{0} Process finished successfully", DateTime.Now);
    }
}