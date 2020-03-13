using Xunit;

namespace Nu.Plugin.Tests
{
    public class NuPluginTests
    {
        private readonly NuPlugin _nuPlugin = NuPlugin.Build("name");

        [Fact]
        public void NuPlugin_Build_Returns_NuPlugin()
        {
            // Arrange & Act
            var nuPlugin = NuPlugin.Build("name");

            // Assert
            Assert.NotNull(nuPlugin);
        }

        [Fact]
        public void NuPlugin_Description_Returns_NuPlugin()
        {
            // Arrange & Act
            var nuPlugin = _nuPlugin.Description("description");

            // Assert
            Assert.NotNull(nuPlugin);
        }
    }
}
