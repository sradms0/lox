using FluentAssertions;
using Frontend;
using Interfaces.Frontend;
using Moq;
using NUnit.Framework;
using Shared;
using Syntax;

namespace Unit.Frontend;

public abstract class LexerTestFixture : CommonTestBase
{
    private static readonly Token ExpectedEndOfFileToken = new(TokenType.Eof, string.Empty, null!, 1);
    
    protected static readonly IReadOnlyDictionary<char, TokenType> ExpectedCharacterTokenTypeMappings =
        new Dictionary<char, TokenType>
        {
            { '(', TokenType.LeftParen },
            { ')', TokenType.RightParen },
            { '{', TokenType.LeftBrace },
            { '}', TokenType.RightBrace },
            { ',', TokenType.Comma },
            { '.', TokenType.Dot },
            { '-', TokenType.Minus },
            { '+', TokenType.Plus },
            { ';', TokenType.Semicolon },
            { '*', TokenType.Star }
        };

    protected static readonly IList<char> TokenCharacters = ExpectedCharacterTokenTypeMappings.Keys
        .Select(tokenCharacter => tokenCharacter).ToList();
    
    protected string Source { get; set; } = null!;

    protected Mock<IErrorHandler> MockErrorHandler { get; private set; } = null!;
    
    protected Lexer Lexer { get; private set; } = null!;
    
    [SetUp]
    public void SetUp()
    {
        Source = Create<string>();

        MockErrorHandler = new Mock<IErrorHandler>();
        
        Lexer = new Lexer(MockErrorHandler.Object);
    }
    
    protected List<char> CreatePreBuiltTokenTokenSource(bool hasOneInvalidTokenCharacter, bool hasManyValidTokenCharacters, bool hasAllInvalidTokenCharacters)
    {
        AddCustomization(ExpectedCharacterTokenTypeMappings.Keys);
        var preBuiltSource = CreateMany<char>().ToList();
        var invalidTokenCharacters = Enumerable.Range(0, preBuiltSource.Count)
            .Select(_ => GetRandomNonTokenCharacter()).ToList();
        
        var maxIndex = CreateMaxIndexFromOneToManyRequest(hasOneInvalidTokenCharacter, hasManyValidTokenCharacters, 
            hasAllInvalidTokenCharacters, invalidTokenCharacters.Count);
        for (var index = 0; index < maxIndex; index++)
        {
            preBuiltSource[index] = invalidTokenCharacters[index];
        }
        
        return preBuiltSource;
    }

    protected static IEnumerable<Token> CreateExpectedTokenResultFromSource(IEnumerable<char> preBuiltSource)
    {
        var expectedTokenResult = preBuiltSource.Select(tokenCharacter =>
        {
            return ExpectedCharacterTokenTypeMappings.TryGetValue(tokenCharacter, out var tokenType)
                ? new Token(tokenType, tokenCharacter.ToString(), null!, 1)
                : null;
        }).OfType<Token>();
        
        return [..expectedTokenResult, ExpectedEndOfFileToken];
    }

    protected static int DetermineErrorHandlerHandleCallCount(IEnumerable<char> preBuiltSource)
    {
        return preBuiltSource.Count(sourceCharacter =>
        {
            return !ExpectedCharacterTokenTypeMappings.TryGetValue(sourceCharacter, out _);
        });
    }
    
    protected static List<char> Shuffle(List<char> source)
    {
        var shuffledSource = source.Select(_ => (char?)null).ToList();
        var indexes = Enumerable.Range(0, source.Count).ToList();
        List<int> sourceIndexes = [..indexes];
        
        sourceIndexes.ForEach(sourceIndex =>
        {
            var randomIndex = indexes[Random.Shared.Next(indexes.Count)];
            shuffledSource[randomIndex] = source[sourceIndex];
            indexes.Remove(randomIndex);

        });
        
        return shuffledSource.OfType<char>().ToList();
    }
    
    protected static void AssertTokenEquivalence(IEnumerable<Token> resultingTokens, IEnumerable<Token> expectedTokens)
    {
        resultingTokens
            .Zip(expectedTokens, (resultingToken, expectedToken) => (resultingToken, expectedToken)).ToList()
            .ForEach(zippedTokens =>
            {
                AssertTokenEquivalence(zippedTokens.resultingToken, zippedTokens.expectedToken);
            });
    }

    private static void AssertTokenEquivalence(Token resultingToken, Token expectedToken)
    {
        resultingToken.ToString().Should().Be(expectedToken.ToString());
    }

    private static int CreateMaxIndexFromOneToManyRequest(bool hasOne, bool hasMany, bool hasAll, int sourceCount)
    {
        var maxIndex = 0;
        if (hasOne)
        {
            maxIndex++;
        }
        else if (hasMany)
        {
            maxIndex = sourceCount - 1;
        }
        else if (hasAll)
        {
            maxIndex = sourceCount;
        }
        
        return maxIndex;
    }
    
    private static char GetRandomNonTokenCharacter()
    {
        char randomNonTokenCharacter;
        do
        {
            randomNonTokenCharacter = (char)Random.Shared.Next();
        } while (ExpectedCharacterTokenTypeMappings.TryGetValue(randomNonTokenCharacter, out _));
        
        return randomNonTokenCharacter;
    }
}