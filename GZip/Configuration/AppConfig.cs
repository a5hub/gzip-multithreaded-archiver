using System;
using Microsoft.Extensions.Configuration;

namespace FileProcessor.Configuration
{
    /// <summary> Application configuration </summary>
    public class AppConfig : IAppConfig
    {
        /// <summary> Amount of threads to be created </summary>
        private readonly int EnvironmentProcessors = Environment.ProcessorCount;

        /// <summary> Microsoft configuration provider </summary>
        private readonly IConfiguration _configuration;

        /// <summary> Application configuration </summary>
        /// <param name="configuration"> Microsoft configuration provider </param>
        public AppConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary> Do app need to count processors automatically </summary>
        public bool AutoCountProcessors
            => _configuration.GetSection("Threading").GetValue<bool>("AutoCountProcessors");

        /// <summary> Processors count form configuration file </summary>
        private int ManualProcessorsCount
            => _configuration.GetSection("Threading").GetValue<int>("ProcessorsCount");

        /// <summary> Size of compression chunk </summary>
        public int CompressionChunkSize
            => _configuration.GetSection("Compression").GetValue<int>("ChunkSize");

        /// <summary> If manual processors count chosen, gets its value,
        /// but not more than environment logical processors count </summary>
        public int ProcessorsCount 
            => AutoCountProcessors 
                ? EnvironmentProcessors
                : ManualProcessorsCount > EnvironmentProcessors
                    ? EnvironmentProcessors 
                    : ManualProcessorsCount;
    }
}
