using FluentAssertions;
using NUnit.Framework;
using Shared;
using Syntax;

namespace Unit.Syntax;

[TestFixture]
public class TokenTypeShould : CommonTestBase
{
    [Test]
    public void HaveValues()
    {
        // Arrange
        HashSet<string> expectedResult = 
        [
            "LeftParen",
            "RightParen",
            "LeftBrace",
            "RightBrace",
            "Comma",
            "Dot",
            "Minus",
            "Plus",
            "Semicolon",
            "Slash",
            "Star",
            "Bang",
            "BangEqual",
            "Equal",
            "EqualEqual",
            "Greater",
            "GreaterEqual",
            "Less",
            "LessEqual",
            "Identifier",
            "String",
            "Number",
            "And",
            "Class",
            "Else",
            "False",
            "Fun",
            "For",
            "If",
            "Nil",
            "Or",
            "Print",
            "Return",
            "Super",
            "This",
            "True",
            "Var",
            "While",
            "Eof"
        ];
        
        // Act
        var result = Enum.GetNames<TokenType>();
        
        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }
}
