using FluentAssertions;
using Frontend;
using Interfaces.Frontend;
using NUnit.Framework;

namespace Unit.Frontend;

[TestFixture]
public class InputShould : InputTestFixture
{
    [Test]
    public void Implement_IInput()
    {
        //Arrange
        // Act 
        // Assert
        typeof(Input).Should().Implement<IInput>();
    }
}