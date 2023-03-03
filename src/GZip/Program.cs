using System;
using System.IO;
using System.IO.Abstractions;
using GZip.Common;
using GZip.Configuration;
using GZip.Logic.Archivation;
using GZip.Logic.FileAccess;
using GZip.Logic.Operations;
using GZip.Logic.TaskManagement;
using GZip.Logic.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace GZip
{
    class Program
    {
        private static IServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            try
            {
                RegisterServices();
                var gzipProcessor = _serviceProvider.GetService<IGZipProcessor>();
                gzipProcessor.Run(args);
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
            collection.AddScoped<IFileAccessor, FileAccessor>();
            collection.AddScoped<ITaskManager, TaskManager>();
            collection.AddScoped<IThreadProvider, ThreadProvider>();
            collection.AddScoped<IThreadManager, ThreadManager>();
            collection.AddScoped<IGZipProcessor, GZipProcessor>();
            collection.AddScoped<ITaskProcessLogic, TaskProcessLogic>();
            collection.AddScoped<IOperation, CompressionOperation>();
            collection.AddScoped<IOperation, DecompressionOperation>();
            collection.AddScoped<IOperationFactory, OperationFactory>();
            collection.AddScoped<IAppConfig, AppConfig>();
            collection.AddScoped<IFileSystem, FileSystem>();
            collection.AddScoped<ITaskSynchronizationParamsFactory, TaskSynchronizationParamsFactory>();
            collection.AddScoped<IGZipTaskFactory, GZipTaskFactory>();
            collection.AddScoped<ITaskQueue<GZipTask>, TaskQueue>();

            var builder = new ConfigurationBuilder();
            BuildConfig(builder);
            IConfiguration configuration = builder.Build();

            collection.AddSingleton<IConfiguration>(provider => configuration);

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
}
