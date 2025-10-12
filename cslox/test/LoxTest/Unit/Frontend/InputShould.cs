using FluentAssertions;
using Frontend;
using Interfaces.Frontend;
using Moq;
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

    [TestCase(false)]
    [TestCase(true)]
    public void ReadLine(bool doesReturnNull)
    {
        //Arrange
        var expectedResult = doesReturnNull ? null : Create<string>();
        MockTextReader
            .Setup(x => x.ReadLine())
            .Returns(expectedResult);
        string? result = null;
        
        // Act (define)
        var readLine = () => result = Input.ReadLine();

        // Assert
        readLine.Should().NotThrow();
        result.Should().Be(expectedResult);
        MockTextReader.Verify(reader => reader.ReadLine(), Times.Once);
        MockTextReader.VerifyNoOtherCalls();
    }
}