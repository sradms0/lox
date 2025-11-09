using FluentAssertions;
using Frontend;
using Interfaces.Frontend;
using Moq;
using NUnit.Framework;
using Syntax;

namespace Unit.Frontend;

[TestFixture]
public class LexerShould : LexerTestFixture
{
    [Test]
    public void Implement_ILexer()
    {
        // Arrange
        // Act
        // Assert
        typeof(Lexer).Should().Implement<ILexer>();
    }
    
    [Test, TestCaseSource(nameof(TokenCharacters))]
    public void ReadTokens_From_One_Valid_Token_Source(char tokenCharacter)
    {
        // Arrange
        Source = tokenCharacter.ToString();
        var expectedResult = CreateExpectedTokenResultFromSource([tokenCharacter]);
        IEnumerable<Token>? result = null;
        
        // Act (define)
        var readTokens = () => result = Lexer.ReadTokens(Source);
        
        // Assert
        readTokens.Should().NotThrow();
        AssertTokenEquivalence(result, expectedResult);
        MockErrorHandler.VerifyNoOtherCalls();
    }

    [Test, Combinatorial]
    public void ReadTokens_From_Many_Valid_Token_Character_Source
    (
        [ValueSource(nameof(TokenCharacters))] char tokenCharacter1,
        [ValueSource(nameof(TokenCharacters))] char tokenCharacter2
    )
    {
        // Arrange
        Source = $"{tokenCharacter1}{tokenCharacter2}";
        var expectedResult = CreateExpectedTokenResultFromSource([tokenCharacter1, tokenCharacter2]);
        IEnumerable<Token>? result = null;
        
        // Act (define)
        var readTokens = () => result = Lexer.ReadTokens(Source);
        
        // Assert
        readTokens.Should().NotThrow();
        AssertTokenEquivalence(result, expectedResult);
        MockErrorHandler.VerifyNoOtherCalls();
    }

    [Test]
    public void ReadTokens_From_All_Valid_Token_Character_Source()
    {
        // Arrange
        Source = string.Join(string.Empty, TokenCharacters);
        var expectedResult = CreateExpectedTokenResultFromSource(ExpectedCharacterTokenTypeMappings.Keys);
        IEnumerable<Token>? result = null;

        // Act (define)
        var readTokens = () => result = Lexer.ReadTokens(Source);

        // Assert
        readTokens.Should().NotThrow();
        AssertTokenEquivalence(result, expectedResult);
        MockErrorHandler.VerifyNoOtherCalls();
    }

    [TestCase(true, false, false)]
    [TestCase(false, true, false)]
    [TestCase(false, false, true)]
    public void ReadTokens_From_Invalid_Token_Character_Source(bool hasOneInvalidTokenCharacter, 
        bool hasManyValidTokenCharacters, bool hasAllInvalidTokenCharacters)
    {
        // Arrange
        var preBuiltSource = CreatePreBuiltTokenTokenSource(hasOneInvalidTokenCharacter, 
            hasManyValidTokenCharacters, hasAllInvalidTokenCharacters);
        var shuffledPrebuiltSource = Shuffle(preBuiltSource);
        Source = string.Join(string.Empty, shuffledPrebuiltSource);
        
        const string expectedErrorHandlerMessage = "Unexpected Character.";
        var expectedErrorHandlerErrorCallCount = DetermineErrorHandlerHandleCallCount(shuffledPrebuiltSource);
        var expectedResult = CreateExpectedTokenResultFromSource(shuffledPrebuiltSource);
        IEnumerable<Token>? result = null;

        // Act (define)
        var readTokens = () => result = Lexer.ReadTokens(Source);

        // Assert
        readTokens.Should().NotThrow();
        AssertTokenEquivalence(result, expectedResult);
        MockErrorHandler.Verify(handler => handler.Error(1, expectedErrorHandlerMessage), 
            Times.Exactly(expectedErrorHandlerErrorCallCount));
        MockErrorHandler.VerifyNoOtherCalls();
    }

    [TestCase(false)]
    [TestCase(true)]
    public void ReadTokens_From_Null_Or_Empty_Source(bool isNull)
    {
        // Arrange
        Source = isNull ? null! : string.Empty;
        IEnumerable<Token> expectedResult = [ExpectedEndOfFileToken];
        IEnumerable<Token>? result = null;

        // Act (define)
        var readTokens = () => result = Lexer.ReadTokens(Source);

        // Assert
        readTokens.Should().NotThrow();
        AssertTokenEquivalence(result, expectedResult);
        MockErrorHandler.VerifyNoOtherCalls();
    }
}