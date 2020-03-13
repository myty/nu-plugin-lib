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
            var expectedValue = nameof(Signature.Description);

            // Act
            var newSignature = _signature.WithDescription(expectedValue);

            // Assert
            Assert.Equal(newSignature.Description, expectedValue);
            Assert.NotEqual(_signature, newSignature);
            Assert.NotEqual(_signature.Description, newSignature.Description);
        }

        [Fact]
        public void Signature_WithName_Returns_New_Object()
        {
            // Arrange
            var expectedValue = nameof(Signature.Name);

            // Act
            var newSignature = _signature.WithName(expectedValue);

            // Assert
            Assert.Equal(newSignature.Name, expectedValue);
            Assert.NotEqual(_signature, newSignature);
            Assert.NotEqual(_signature.Name, newSignature.Name);
        }

        [Fact]
        public void Signature_WithIsFilter_Returns_New_Object()
        {
            // Arrange
            var expectedValue = true;

            // Act
            var newSignature = _signature.WithIsFilter(expectedValue);

            // Assert
            Assert.Equal(newSignature.IsFilter, expectedValue);
            Assert.NotEqual(_signature, newSignature);
            Assert.NotEqual(_signature.IsFilter, newSignature.IsFilter);
        }

        [Fact]
        public void Signature_AddSwitch_Returns_New_Object()
        {
            // Arrange
            var nameSwitch = "switch";
            var nameSwitchDescription = "switch description";

            // Act
            var newSignature = _signature.AddSwitch(nameSwitch, nameSwitchDescription);

            // Assert
            Assert.True(newSignature.Named.ContainsKey(nameSwitch));
            Assert.Equal(2, newSignature.Named[nameSwitch].Length);
            Assert.Equal(typeof(NamedTypeSwitch), newSignature.Named[nameSwitch][0].GetType());
            Assert.Equal(nameSwitchDescription, newSignature.Named[nameSwitch][1]);
        }

        [Fact]
        public void Signature_AddOptionalNamed_Returns_New_Object()
        {
            // Arrange
            var nameOptional = "optional";
            var nameOptionalDescription = "optional description";
            var nameOptionalShape = SyntaxShape.Int;

            // Act
            var newSignature = _signature.AddOptionalNamed(nameOptionalShape, nameOptional, nameOptionalDescription);

            // Assert
            Assert.True(newSignature.Named.ContainsKey(nameOptional));

            var optionalNamedParameter = newSignature.Named[nameOptional];
            Assert.Equal(2, optionalNamedParameter.Length);
            Assert.Equal(nameOptionalDescription, optionalNamedParameter[1]);

            var actualOptionalObj = optionalNamedParameter[0] as NamedTypeOptional;
            Assert.NotNull(actualOptionalObj);
            Assert.Equal("o", actualOptionalObj.Optional[0]);
            Assert.Equal(nameOptionalShape.Shape, actualOptionalObj.Optional[1]);
        }

        [Fact]
        public void Signature_AddRequiredNamed_Returns_New_Object()
        {
            // Arrange
            var nameOptional = "required";
            var nameOptionalDescription = "required description";
            var nameOptionalShape = SyntaxShape.Path;

            // Act
            var newSignature = _signature.AddRequiredNamed(nameOptionalShape, nameOptional, nameOptionalDescription);

            // Assert
            Assert.True(newSignature.Named.ContainsKey(nameOptional));

            var optionalNamedParameter = newSignature.Named[nameOptional];
            Assert.Equal(2, optionalNamedParameter.Length);
            Assert.Equal(nameOptionalDescription, optionalNamedParameter[1]);

            var actualOptionalObj = optionalNamedParameter[0] as NamedTypeMandatory;
            Assert.NotNull(actualOptionalObj);
            Assert.Equal("r", actualOptionalObj.Mandatory[0]);
            Assert.Equal(nameOptionalShape.Shape, actualOptionalObj.Mandatory[1]);
        }

        [Fact]
        public void Signature_Create_Returns_Default_Values()
        {
            // Arrange
            var expectedDescriptionValue = nameof(Signature.Description);
            var expectedNameValue = nameof(Signature.Name);
            var expectedIsFilterValue = true;

            // Act
            var newSignature = _signature
                .WithDescription(expectedDescriptionValue)
                .WithName(expectedNameValue)
                .WithIsFilter(expectedIsFilterValue);

            // Assert
            Assert.NotEqual(_signature, newSignature);
            Assert.Equal(newSignature.Description, expectedDescriptionValue);
            Assert.Equal(newSignature.Name, expectedNameValue);
            Assert.Equal(newSignature.IsFilter, expectedIsFilterValue);
        }
    }
}
