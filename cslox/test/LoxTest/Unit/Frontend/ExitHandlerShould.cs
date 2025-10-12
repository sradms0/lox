using FluentAssertions;
using Frontend;
using Interfaces.Frontend;
using NUnit.Framework;
using Shared;

namespace Unit.Frontend;

[TestFixture]
public class ExitHandlerShould : CommonTestBase
{
    [Test]
    public void Implement_IExitHandler()
    {
        // Arrange
        // Act
        // Assert
        typeof(ExitHandler).Should().Implement<IExitHandler>();
    }
}