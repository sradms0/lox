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
    
    [Test, TestCaseSource(nameof(SingleTokenSymbols))]
    public void ReadTokens_From_One_Valid_Single_Token_Symbol_Source(string singleTokenSymbol)
    {
        // Arrange
        Source = singleTokenSymbol;
        var expectedResult = CreateExpectedSingleTokenResultFromSource([singleTokenSymbol]);
        IEnumerable<Token>? result = null;
        
        // Act (define)
        var readTokens = () => result = Lexer.ReadTokens(Source);
        
        // Assert
        readTokens.Should().NotThrow();
        AssertTokenEquivalence(result, expectedResult);
        MockErrorHandler.VerifyNoOtherCalls();
    }

    [Test, TestCaseSource(nameof(MultiTokenSymbols))]
    public void ReadTokens_From_One_Valid_Multi_Token_Symbol_Source(string multiTokenSymbol)
    {
        // Arrange
        Source = multiTokenSymbol;
        var expectedResult = CreateExpectedMultiTokenResultFromSource([multiTokenSymbol]);
        IEnumerable<Token>? result = null;

        // Act (define)
        var readTokens = () => result = Lexer.ReadTokens(Source);

        // Assert
        readTokens.Should().NotThrow();
        AssertTokenEquivalence(result, expectedResult);
        MockErrorHandler.VerifyNoOtherCalls();
    }
    
    [Test, Combinatorial]
    public void ReadTokens_From_Many_Valid_Single_Token_Symbol_Source
    (
        [ValueSource(nameof(SingleTokenSymbolsCombinationTestCaseSource))] StringPair singleTokenSymbolPair
    )
    {
        // Arrange
        var singleTokenSymbol1 = singleTokenSymbolPair.First;
        var singleTokenSymbol2 = singleTokenSymbolPair.Second;
        Source = $"{singleTokenSymbol1}{singleTokenSymbol2}";
        var expectedResult = CreateExpectedSingleTokenResultFromSource([singleTokenSymbol1, singleTokenSymbol2]);
        IEnumerable<Token>? result = null;
        
        // Act (define)
        var readTokens = () => result = Lexer.ReadTokens(Source);
        
        // Assert
        readTokens.Should().NotThrow();
        AssertTokenEquivalence(result, expectedResult);
        MockErrorHandler.VerifyNoOtherCalls();
    }
    
    [Test, Combinatorial]
    public void ReadTokens_From_Many_Valid_Multi_Token_Symbol_Source
    (
        [ValueSource(nameof(MultiTokenSymbols))] string multiTokenSymbol1,
        [ValueSource(nameof(MultiTokenSymbols))] string multiTokenSymbol2
    )
    {
        // Arrange
        Source = $"{multiTokenSymbol1}{multiTokenSymbol2}";
        var expectedResult = CreateExpectedMultiTokenResultFromSource([multiTokenSymbol1, multiTokenSymbol2]);
        IEnumerable<Token>? result = null;
        
        // Act (define)
        var readTokens = () => result = Lexer.ReadTokens(Source);
        
        // Assert
        readTokens.Should().NotThrow();
        AssertTokenEquivalence(result, expectedResult);
        MockErrorHandler.VerifyNoOtherCalls();
    }
    
    [Test]
    public void ReadTokens_From_All_Valid_Single_Token_Symbol_Source()
    {
        // Arrange
        Source = string.Join(string.Empty, SingleTokenSymbols);
        var expectedResult = CreateExpectedSingleTokenResultFromSource(ExpectedSingleTokenSymbolTypeMappings.Keys);
        IEnumerable<Token>? result = null;

        // Act (define)
        var readTokens = () => result = Lexer.ReadTokens(Source);

        // Assert
        readTokens.Should().NotThrow();
        AssertTokenEquivalence(result, expectedResult);
        MockErrorHandler.VerifyNoOtherCalls();
    }
    
    [Test]
    public void ReadTokens_From_All_Valid_Multi_Token_Symbol_Source()
    {
        // Arrange
        Source = string.Join(string.Empty, MultiTokenSymbols);
        var expectedResult = CreateExpectedTokenResultFromSource(MultiTokenSymbols);
        IEnumerable<Token>? result = null;

        // Act (define)
        var readTokens = () => result = Lexer.ReadTokens(Source);

        // Assert
        readTokens.Should().NotThrow();
        AssertTokenEquivalence(result, expectedResult);
        MockErrorHandler.VerifyNoOtherCalls();
    }

    [Test, Combinatorial]
    public void ReadTokens_From_Many_Single_And_Multi_Token_Symbol_Source
    (
        [ValueSource(nameof(SingleTokenSymbols))] string singleTokenSymbol1,
        [ValueSource(nameof(MultiTokenSymbols))] string multiTokenSymbol2
    )
    {
        // Arrange
        Source = $"{singleTokenSymbol1}{multiTokenSymbol2}";
        var expectedResult = CreateExpectedTokenResultFromSource([singleTokenSymbol1, multiTokenSymbol2]);
        IEnumerable<Token>? result = null;
        
        // Act (define)
        var readTokens = () => result = Lexer.ReadTokens(Source);
        
        // Assert
        readTokens.Should().NotThrow();
        AssertTokenEquivalence(result, expectedResult);
        MockErrorHandler.VerifyNoOtherCalls();
    }

    [Test]
    public void ReadTokens_From_All_Valid_Token_Symbol_Source()
    {
        // Arrange
        Source = string.Join(string.Empty, AllTokenSymbols);
        var expectedResult = CreateExpectedTokenResultFromSource(ExpectedTokenSymbolMappings.Keys);
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
    public void ReadTokens_From_Invalid_Single_Token_Symbol_Source(bool hasOneInvalidTokenSymbol, 
        bool hasManyValidTokenSymbols, bool hasAllInvalidTokenSymbols)
    {
        // Arrange
        var preBuiltSource = CreatePreBuiltSingleTokenSymbolSource(hasOneInvalidTokenSymbol, 
            hasManyValidTokenSymbols, hasAllInvalidTokenSymbols);
        var shuffledPrebuiltSource = Shuffle(preBuiltSource);
        Source = string.Join(string.Empty, shuffledPrebuiltSource);
        
        var expectedErrorHandlerErrorCallCount = DetermineSingleTokenSymbolBasedErrorHandlerHandleCallCount(shuffledPrebuiltSource);
        var expectedResult = CreateExpectedSingleTokenResultFromSource(shuffledPrebuiltSource);
        IEnumerable<Token>? result = null;

        // Act (define)
        var readTokens = () => result = Lexer.ReadTokens(Source);

        // Assert
        readTokens.Should().NotThrow();
        AssertTokenEquivalence(result, expectedResult);
        MockErrorHandler.Verify(handler => handler.Error(1, ExpectedInvalidCharacterMessage), 
            Times.Exactly(expectedErrorHandlerErrorCallCount));
        MockErrorHandler.VerifyNoOtherCalls();
    }
    
    [TestCase(true, false, false)]
    [TestCase(false, true, false)]
    [TestCase(false, false, true)]
    public void ReadTokens_From_Invalid_Multi_Token_Symbol_Source(bool hasOneInvalidTokenSymbol, 
        bool hasManyValidTokenSymbols, bool hasAllInvalidTokenSymbols)
    {
        // Arrange
        var preBuiltSource = CreatePreBuiltMultiTokenSymbolSource(hasOneInvalidTokenSymbol, 
            hasManyValidTokenSymbols, hasAllInvalidTokenSymbols);
        var shuffledPrebuiltSource = Shuffle(preBuiltSource);
        Source = string.Join(string.Empty, shuffledPrebuiltSource);
        
        var expectedErrorHandlerErrorCallCount = DetermineMultiTokenSymbolBasedErrorHandlerHandleCallCount(shuffledPrebuiltSource);
        var expectedResult = CreateExpectedMultiTokenResultFromSource(shuffledPrebuiltSource);
        IEnumerable<Token>? result = null;

        // Act (define)
        var readTokens = () => result = Lexer.ReadTokens(Source);

        // Assert
        readTokens.Should().NotThrow();
        AssertTokenEquivalence(result, expectedResult);
        MockErrorHandler.Verify(handler => handler.Error(1, ExpectedInvalidCharacterMessage), 
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

    [TestCase("/,/,\n")]
    [TestCase("/,/,\n,/,/,\n")]
    [TestCase("/,/,\n,/")]
    [TestCase("/,/,\n/,/,\n,/")]
    [TestCase("/,\n,/,/,\n,/,/,\n")]
    [TestCase("/,\n,/,/,\n,/,/,\n,/")]
    [TestCase("/,/,/,/\n")]
    [TestCase("/,\n,/,/,/,/,\n")]
    [TestCase("/,/,/,/,\n,/")]
    [TestCase("/,/")]
    [TestCase("/,/,/,/")]
    public void ReadTokens_From_Comment_Included_Source(string commentIncludedSource)
    {
        // Arrange
        var preBuiltSource = commentIncludedSource.Split(",");
        Source = string.Join(string.Empty, preBuiltSource);
        var expectedResult = CreateExpectedTokenResultFromSource(preBuiltSource);
        IEnumerable<Token>? result = null;
        
        // Act 
        var readTokens = () => result = Lexer.ReadTokens(Source);

        // Assert
        readTokens.Should().NotThrow();
        AssertTokenEquivalence(result, expectedResult);
    }
}