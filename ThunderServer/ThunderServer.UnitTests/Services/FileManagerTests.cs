using Xunit;
using System.IO;
using ThunderServer.API.Services;
using ThunderServer.API.Configurations;
using Moq;
using Microsoft.Extensions.Options;
using System.Numerics;
using ThunderServer.Infratructure.Storage;
using Microsoft.Extensions.Logging;

namespace ThunderServer.Tests.Services
{
    public class FileManagerTests
    {
        private readonly FileManager fileManager;
        private readonly StorageVolumesConfiguration configuration;

        public FileManagerTests()
        {
            fileManager = new FileManager(Mock.Of<IOptionsMonitor<StorageVolumesConfiguration>>(), Mock.Of<ILogger<FileManager>>(), Mock.Of<ThunderServerContext>());
        }


        [Theory]
        [InlineData("testfile.txt", false)]
        [InlineData("duplicatefile.txt", true)]
        public void IsDuplicateFile_ReturnsExpectedResult(string fileName, bool expectedResult)
        {
            // Arrange
            string directory = Path.Combine(Directory.GetCurrentDirectory(), "TestFiles");
            string filePath = Path.Combine(directory, fileName);

            // Act
            bool result = FileManager.IsDuplicateFile(filePath);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CalculateChecksum_ReturnsExpectedChecksum()
        {
            // Arrange
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "TestFiles", "testfile.txt");
            string expectedChecksum = "098f6bcd4621d373cade4e832627b4f6"; // MD5 checksum for "testfile"

            // Act
            string checksum = FileManager.CalculateChecksum(filePath);

            // Assert
            Assert.Equal(expectedChecksum, checksum);
        }
    }
}
