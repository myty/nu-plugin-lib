using Xunit;

namespace Nu.Plugin.Tests
{
    public class SignatureTests
    {
        private readonly Signature _signature = Signature.Create();

        [Fact]
        public void Signature_Create_Returns_New_Object()
        {
            // Arrange & Act & Assert
            Assert.NotNull(_signature);
        }

        [Fact]
        public void Signature_WithDescription_Returns_New_Object()
        {
            // Arrange
            const string expectedValue = nameof(Signature.Description);

            // Act
            var newSignature = _signature.WithDescription(expectedValue);

            // Assert
            Assert.Equal(newSignature.Description, expectedValue);
            Assert.NotEqual(_signature,             newSignature);
            Assert.NotEqual(_signature.Description, newSignature.Description);
        }

        [Fact]
        public void Signature_WithName_Returns_New_Object()
        {
            // Arrange
            const string expectedValue = nameof(Signature.Name);

            // Act
            var newSignature = _signature.WithName(expectedValue);

            // Assert
            Assert.Equal(newSignature.Name, expectedValue);
            Assert.NotEqual(_signature,      newSignature);
            Assert.NotEqual(_signature.Name, newSignature.Name);
        }

        [Fact]
        public void Signature_WithIsFilter_Returns_New_Object()
        {
            // Arrange
            const bool expectedValue = true;

            // Act
            var newSignature = _signature.WithIsFilter(expectedValue);

            // Assert
            Assert.Equal(newSignature.IsFilter, expectedValue);
            Assert.NotEqual(_signature,          newSignature);
            Assert.NotEqual(_signature.IsFilter, newSignature.IsFilter);
        }

        [Fact]
        public void Signature_AddNamedSwitch_Returns_New_Object()
        {
            // Arrange
            const string nameSwitch = "switch";
            const string nameSwitchDescription = "switch description";

            // Act
            var newSignature = _signature.AddNamedSwitch(nameSwitch, nameSwitchDescription);

            // Assert
            Assert.True(newSignature.Named.ContainsKey(nameSwitch));
            Assert.Equal(2, newSignature.Named[nameSwitch].Length);
            Assert.Equal(typeof(NamedTypeSwitch), newSignature.Named[nameSwitch][0].GetType());
            Assert.Equal(nameSwitchDescription, newSignature.Named[nameSwitch][1]);
        }

        [Fact]
        public void Signature_Create_Returns_Default_Values()
        {
            // Arrange
            const string expectedDescriptionValue = nameof(Signature.Description);
            const string expectedNameValue        = nameof(Signature.Name);
            const bool   expectedIsFilterValue    = true;

            // Act
            var newSignature = _signature
                .WithDescription(expectedDescriptionValue)
                .WithName(expectedNameValue)
                .WithIsFilter(expectedIsFilterValue);

            // Assert
            Assert.NotEqual(_signature, newSignature);

            Assert.Equal(newSignature.Description, expectedDescriptionValue);
            Assert.Equal(newSignature.Name,        expectedNameValue);
            Assert.Equal(newSignature.IsFilter,    expectedIsFilterValue);
        }
    }
}
