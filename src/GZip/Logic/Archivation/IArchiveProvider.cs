namespace GZip.Logic.Archivation
{
    /// <summary> Logic provides methods to compress and decompress data </summary>
    public interface IArchiveProvider
    {
        /// <summary> Compresses byte array </summary>
        /// <param name="inputBytes"> Input byte array to compress </param>
        /// <returns> Compressed array </returns>
        public byte[] Compress(byte[] inputBytes);

        /// <summary> Decompresses byte array </summary>
        /// <param name="inputBytes"> Input byte array to decompress </param>
        /// <returns> Decompressed array </returns>
        public byte[] Decompress(byte[] inputBytes);
    }
}
