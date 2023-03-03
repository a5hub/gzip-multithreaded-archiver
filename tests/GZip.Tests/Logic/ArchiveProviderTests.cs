using System.Text;
using GZip.Logic.Archivation;
using Xunit;

namespace GZip.Tests.Logic
{
    public class ArchiveProviderTests
    {
        private readonly ArchiveProvider target;

        public ArchiveProviderTests()
        {
            target = new ArchiveProvider();
        }

        [Fact]
        public void CompressTest_EmptyArray()
        {
            // arrange
            var input = new byte[] { };
            var expected = new byte[] { };

            // act
            var actual = target.Compress(input);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CompressTest_WorksFine()
        {
            // arrange
            var name = "TestCompressionString";  
            var input = Encoding.ASCII.GetBytes(name);
            var expected = new byte[]
            {
                31, 139, 8, 0, 0, 0, 0, 0, 0, 10, 11, 73, 45, 46, 113, 206, 207, 
                45, 40, 74, 45, 46, 206, 204, 207, 11, 46, 41, 202, 204, 75, 7, 0, 
                75, 246, 179, 6, 21, 0, 0, 0
            };

            // act
            var actual = target.Compress(input);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DecompressTest_EmptyArray()
        {
            // arrange
            var input = new byte[] { };
            var expected = new byte[] { };

            // act
            var actual = target.Decompress(input);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DecompressTest_WorksFine()
        {
            // arrange
            var input = new byte[]
            {
                31, 139, 8, 0, 0, 0, 0, 0, 0, 10, 11, 73, 45, 46, 113, 206, 207, 
                45, 40, 74, 45, 46, 206, 204, 207, 11, 46, 41, 202, 204, 75, 7, 0, 
                75, 246, 179, 6, 21, 0, 0, 0
            };
            var expected = "TestCompressionString";

            // act
            var actualBytes = target.Decompress(input);
            var actual = Encoding.ASCII.GetString(actualBytes);

            // assert
            Assert.Equal(expected, actual);
        }
    }
}
