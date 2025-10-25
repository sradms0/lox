using FluentAssertions;
using Frontend;
using Interfaces.Frontend;
using NUnit.Framework;

namespace Unit.Frontend;

[TestFixture]
public class ErrorHandlerShould : ErrorHandlerTestFixture
{
    [Test]
    public void Implement_IErrorHandler()
    {
        // Arrange
        // Act
        // Assert
        typeof(ErrorHandler).Should().Implement<IErrorHandler>();
    }
}