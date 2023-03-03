using System;
using System.IO;
using System.IO.Abstractions;
using GZip.Common.Common;
using GZip.ConsoleApp.Adapter.Services;
using GZip.ConsoleApp.Services;
using GZip.FileAccess.Services;
using GZip.Logic.Configuration;
using GZip.Logic.Logic.Archivation;
using GZip.Logic.Logic.Operations;
using GZip.Logic.Logic.TaskManagement;
using GZip.Logic.Logic.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace GZip.ConsoleApp;

class Program
{
    private static IServiceProvider _serviceProvider;

    static void Main(string[] args)
    {
        try
        {
            RegisterServices();
            var archivationRunner = _serviceProvider.GetService<IArchivationRunnerService>();
            archivationRunner.Run(args);
            EnvironmentControl.Current.Exit(1);
        }
        catch (Exception ex)
        {
            Log.Information($"{ex}");
            Console.WriteLine(ex.Message);
            EnvironmentControl.Current.Exit(0);
        }
        finally
        {
            DisposeServices();
        }
    }

    #region Service registration

    static void BuildConfig(IConfigurationBuilder builder)
    {
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
    }

    private static void RegisterServices()
    {
        var collection = new ServiceCollection();

        collection.AddScoped<IArchiveProvider, ArchiveProvider>();
        collection.AddScoped<IFileAccessService, FileAccessService>();
        collection.AddScoped<ITaskManager, TaskManager>();
        collection.AddScoped<IThreadProvider, ThreadProvider>();
        collection.AddScoped<IThreadManager, ThreadManager>();
        collection.AddScoped<IArchivationRunnerService, ArchivationRunnerService>();
        collection.AddScoped<ITaskProcessLogic, TaskProcessLogic>();
        collection.AddScoped<IOperation, CompressionOperation>();
        collection.AddScoped<IOperation, DecompressionOperation>();
        collection.AddScoped<IOperationFactory, OperationFactory>();
        collection.AddScoped<IAppConfig, AppConfig>();
        collection.AddScoped<IFileSystem, FileSystem>();
        collection.AddScoped<ITaskSynchronizationParamsFactory, TaskSynchronizationParamsFactory>();
        collection.AddScoped<IGZipTaskFactory, GZipTaskFactory>();
        collection.AddScoped<ITaskQueue<GZipTask>, TaskQueue>();
        collection.AddScoped<IArchiveProcessorService, ConsoleArchiveProcessorServiceV1>();

        var builder = new ConfigurationBuilder();
        BuildConfig(builder);
        IConfiguration configuration = builder.Build();

        collection.AddSingleton(_ => configuration);

        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("log.txt")
            .CreateLogger();

        _serviceProvider = collection.BuildServiceProvider();
    }

    private static void DisposeServices()
    {
        if (_serviceProvider == null)
        {
            return;
        }

        if (_serviceProvider is IDisposable)
        {
            ((IDisposable) _serviceProvider).Dispose();
        }
    }

    #endregion Service registration
}