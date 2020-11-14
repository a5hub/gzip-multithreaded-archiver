using FileProcessor.Configuration;
using FileProcessor.Logic.Archivation;
using System;
using System.IO.Abstractions;
using FileProcessor.Common;
using GZip;

namespace FileProcessor.Logic.Operations
{
    /// <summary> Creates operation implementation dependent upon type </summary>
    public class OperationFactory : IOperationFactory
    {
        /// <summary> Application configuration  </summary>
        private readonly IAppConfig _appConfig;

        /// <summary> Logic provides methods to compress and decompress data </summary>
        private readonly IArchiveProvider _archiveProvider;

        /// <summary> Logic provides methods to compress and decompress data </summary>
        private readonly IFileSystem _fileSystem;
        
        /// <summary> Creates operation implementation dependent upon type </summary>
        public OperationFactory(IAppConfig appConfig,
            IArchiveProvider archiveProvider,
            IFileSystem fileSystem)
        {
            _appConfig = appConfig;
            _archiveProvider = archiveProvider;
            _fileSystem = fileSystem;
        }

        /// <summary> Creates operation implementation dependent upon type </summary>
        /// <param name="operationType"> Type of operation to create </param>
        /// <returns> Concrete operation implementation </returns>
        public IOperation Create(OperationType operationType)
        {
            Guard.DefinedEnumValue(operationType);

            switch (operationType)
            {
                case OperationType.Compress:
                    return new CompressionOperation(_appConfig, _archiveProvider, _fileSystem);

                case OperationType.Decompress:
                    return new DecompressionOperation(_appConfig, _archiveProvider, _fileSystem);

                default:
                    throw new ArgumentException(Resources.UnexpectedOperationException);
            }
        }
    }
}
