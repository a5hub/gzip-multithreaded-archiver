using System.IO;
using System.IO.Compression;
using GZip.Common;

namespace GZip.Logic.Archivation
{
    /// <summary> Logic provides methods to compress and decompress data </summary>
    public class ArchiveProvider : IArchiveProvider
    {
        /// <summary> Compresses byte array </summary>
        /// <param name="inputBytes"> Input byte array to compress </param>
        /// <returns> Compressed array </returns>
        public byte[] Compress(byte[] inputBytes)
        {
            Guard.NotNull(inputBytes, $"{nameof(inputBytes)}");

            using MemoryStream input = new MemoryStream(inputBytes);
            using MemoryStream output = new MemoryStream();
            using (var zip = new GZipStream(output, CompressionMode.Compress))
            {
                input.CopyTo(zip);
            }
            return output.ToArray();
        }

        /// <summary> Decompresses byte array </summary>
        /// <param name="inputBytes"> Input byte array to decompress </param>
        /// <returns> Decompressed array </returns>
        public byte[] Decompress(byte[] inputBytes)
        {
            Guard.NotNull(inputBytes, $"{nameof(inputBytes)}");

            using MemoryStream input = new MemoryStream(inputBytes);
            using MemoryStream output = new MemoryStream();
            using (GZipStream zip = new GZipStream(input, CompressionMode.Decompress))
            {
                zip.CopyTo(output);
            }
            return output.ToArray();
        }
    }
}
