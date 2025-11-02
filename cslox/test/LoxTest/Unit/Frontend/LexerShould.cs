using FluentAssertions;
using Frontend;
using Interfaces.Frontend;
using NUnit.Framework;
using Shared;

namespace Unit.Frontend;

[TestFixture]
public class LexerShould : CommonTestBase
{
    [Test]
    public void Implement_ILexer()
    {
        // Arrange
        // Act
        // Assert
        typeof(Lexer).Should().Implement<ILexer>();
    }
}