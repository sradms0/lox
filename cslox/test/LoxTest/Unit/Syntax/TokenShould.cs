using FluentAssertions;
using NUnit.Framework;

namespace Unit.Syntax;

[TestFixture]
public class TokenShould : TokenTestFixture
{
    [Test]
    public void Override_ToString()
    {
        // Arrange
        var expectedResult = $"{Type} ${Lexeme} {Literal}";
        string? result = null;
        
        // Act (define)
        var toString = () => result = Token.ToString();
        
        // Assert
        toString.Should().NotThrow();
        result.Should().Be(expectedResult);
    }
}