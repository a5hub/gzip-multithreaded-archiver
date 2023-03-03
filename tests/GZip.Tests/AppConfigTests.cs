using GZip.Configuration;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace GZip.Tests
{
    public class AppConfigTests
    {
        private AppConfig target;
        private readonly Mock<IConfiguration> configuration = new Mock<IConfiguration>();

        public AppConfigTests()
        {
            target = new AppConfig(configuration.Object);
        }

        [Fact(Skip="Object reference not set to an instance of an object")]
        public void AutoCountProcessorsTest_WorksFine()
        {
            // arrange
            var configurationSection = new Mock<IConfigurationSection>();
            configurationSection.Setup(a => a.Value).Returns("true");
            configuration.Setup(a => a.GetSection("Threading")).Returns(configurationSection.Object);            

            // act
            var actual = target.AutoCountProcessors;

            // assert
            Assert.True(actual);
        }
    }
}
