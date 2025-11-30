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
    public sealed record StringPair(string First, string Second);
    
    protected const string ExpectedInvalidCharacterMessage = "Unexpected Character.";
    
    protected static readonly Token ExpectedEndOfFileToken = new(TokenType.Eof, string.Empty, null!, 1);
    
    protected static readonly IReadOnlyDictionary<string, TokenType> ExpectedSingleTokenSymbolTypeMappings =
        new Dictionary<string, TokenType>
        {
            { "(", TokenType.LeftParen },
            { ")", TokenType.RightParen },
            { "{", TokenType.LeftBrace },
            { "}", TokenType.RightBrace },
            { ",", TokenType.Comma },
            { ".", TokenType.Dot },
            { "-", TokenType.Minus },
            { "+", TokenType.Plus },
            { ";", TokenType.Semicolon },
            { "*", TokenType.Star },
            { "=", TokenType.Equal },
            { "<", TokenType.Less },
            { ">", TokenType.Greater },
            { "!", TokenType.Bang }
        };
    
    protected static readonly IReadOnlyDictionary<string, TokenType> ExpectedMultiTokenSymbolTypeMappings =
        new Dictionary<string, TokenType>
        {
            { "!=", TokenType.BangEqual },
            { "==", TokenType.EqualEqual },
            { "<=", TokenType.LessEqual },
            { ">=", TokenType.GreaterEqual }
        };

    protected static readonly IReadOnlyDictionary<string, TokenType> ExpectedTokenSymbolMappings =
        new Dictionary<string, TokenType>
        ([
            ..ExpectedSingleTokenSymbolTypeMappings, ..ExpectedMultiTokenSymbolTypeMappings
        ]);

    protected static readonly IList<string> SingleTokenSymbols = ExpectedSingleTokenSymbolTypeMappings.Keys.ToList();

    protected static readonly IList<string> MultiTokenSymbols = ExpectedMultiTokenSymbolTypeMappings.Keys .ToList();

    protected static readonly IList<string> AllTokenSymbols = ExpectedTokenSymbolMappings.Keys.ToList();

    protected static IEnumerable<StringPair> SingleTokenSymbolsCombinationTestCaseSource()
    {
        foreach (var singleTokenSymbol1 in SingleTokenSymbols)
        foreach (var singleTokenSymbol2 in SingleTokenSymbols)
        {
            var combinedSymbols = $"{singleTokenSymbol1}{singleTokenSymbol2}";
            if (ExpectedMultiTokenSymbolTypeMappings.ContainsKey(combinedSymbols))
            {
                continue;
            }

            yield return new StringPair(singleTokenSymbol1, singleTokenSymbol2);
        }
    }
    
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

    protected List<string> CreatePreBuiltMultiTokenSymbolSource(bool hasOneInvalidMultiTokenSymbols,
        bool hasManyValidMultiTokenSymbols, bool hasAllInvalidMultiTokenSymbols)
    {
        return CreatePreBuiltTokenSymbolSource(hasOneInvalidMultiTokenSymbols, hasManyValidMultiTokenSymbols,
            hasAllInvalidMultiTokenSymbols, ExpectedMultiTokenSymbolTypeMappings);
    }
    
    protected List<string> CreatePreBuiltSingleTokenSymbolSource(bool hasOneInvalidTokenSymbol, bool hasManyValidTokenSymbols,
        bool hasAllInvalidTokenSymbols)
    {
        return CreatePreBuiltTokenSymbolSource(hasOneInvalidTokenSymbol, hasManyValidTokenSymbols,
            hasAllInvalidTokenSymbols, ExpectedSingleTokenSymbolTypeMappings);
    }

    protected static IEnumerable<Token> CreateExpectedTokenResultFromSource(IEnumerable<string> preBuiltSource)
    {
        var preBuiltSourceList = preBuiltSource.ToList();
        var expectedResult = CreateExpectedTokenResultFromSource(preBuiltSourceList, ExpectedTokenSymbolMappings).ToList();
        PrioritizeIntersectedMultiTokensFromSource(preBuiltSourceList, expectedResult);
        
        return expectedResult;
    }
    
    protected static IEnumerable<Token> CreateExpectedSingleTokenResultFromSource(IEnumerable<string> preBuiltSource)
    {
        return CreateExpectedTokenResultFromSource(preBuiltSource, ExpectedSingleTokenSymbolTypeMappings);
    }
  
    protected static IEnumerable<Token> CreateExpectedMultiTokenResultFromSource(IEnumerable<string> preBuiltSource)
    {
        return CreateExpectedTokenResultFromSource(preBuiltSource, ExpectedMultiTokenSymbolTypeMappings);
    }

    protected static int DetermineMultiTokenSymbolBasedErrorHandlerHandleCallCount(IEnumerable<string> preBuiltSource)
    {
        return DetermineErrorHandlerHandleCallCount(preBuiltSource, ExpectedMultiTokenSymbolTypeMappings);
    }
    
    protected static int DetermineSingleTokenSymbolBasedErrorHandlerHandleCallCount(IEnumerable<string> preBuiltSource)
    {
        return DetermineErrorHandlerHandleCallCount(preBuiltSource, ExpectedSingleTokenSymbolTypeMappings);
    }

    protected static List<string> Shuffle(List<string> source)
    {
        var shuffledSource = source.Select(string? (_) => null).ToList();
        var indexes = Enumerable.Range(0, source.Count).ToList();
        List<int> sourceIndexes = [..indexes];
        
        sourceIndexes.ForEach(sourceIndex =>
        {
            var randomIndex = indexes[Random.Shared.Next(indexes.Count)];
            shuffledSource[randomIndex] = source[sourceIndex];
            indexes.Remove(randomIndex);
        });
        
        return shuffledSource.OfType<string>().ToList();
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
    
    private static IEnumerable<Token> CreateExpectedTokenResultFromSource(IEnumerable<string> preBuiltSource, IReadOnlyDictionary<string, TokenType> tokenSymbolTypeMappings)
    {
        var expectedTokenResult = preBuiltSource.Select(tokenSymbol =>
        {
            return tokenSymbolTypeMappings.TryGetValue(tokenSymbol, out var tokenType)
                ? new Token(tokenType, tokenSymbol, null!, 1)
                : null;
        }).OfType<Token>();
        
        return [..expectedTokenResult, ExpectedEndOfFileToken];
    }
    
    private List<string> CreatePreBuiltTokenSymbolSource(bool hasOneInvalidTokenSymbol,
        bool hasManyValidTokenSymbols, bool hasAllInvalidTokenSymbols,
        IReadOnlyDictionary<string, TokenType> tokenSymbolTypeMappings)
    {
        ClearCustomizations();
        AddCustomization(tokenSymbolTypeMappings.Keys);
        var preBuiltSource = CreateMany<string>().ToList();
        var invalidTokenSymbols = Enumerable.Range(0, preBuiltSource.Count)
            .Select(_ => GetRandomNonTokenSymbol()).ToList();

        var maxIndex = CreateMaxIndexFromOneToManyRequest(hasOneInvalidTokenSymbol, hasManyValidTokenSymbols,
            hasAllInvalidTokenSymbols, invalidTokenSymbols.Count);
        
        for (var index = 0; index < maxIndex; index++)
        {
            preBuiltSource[index] = invalidTokenSymbols[index];
        }
        
        return preBuiltSource;
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
    
    private static int DetermineErrorHandlerHandleCallCount(IEnumerable<string> preBuiltSource, IReadOnlyDictionary<string, TokenType> tokenSymbolTypeMappings)
    {
        return preBuiltSource.Count(sourceSymbol =>
        {
            return !tokenSymbolTypeMappings.TryGetValue(sourceSymbol, out _);
        });
    }
    
    private static string GetRandomNonTokenSymbol()
    {
        string randomNonTokenSymbol;
        do
        {
            randomNonTokenSymbol = ((char)Random.Shared.Next()).ToString();
        } while (ExpectedSingleTokenSymbolTypeMappings.TryGetValue(randomNonTokenSymbol, out _));
        
        return randomNonTokenSymbol;
    }

    private static void PrioritizeIntersectedMultiTokensFromSource(List<string> preBuiltSource, List<Token> expectedResult)
    {
        for (var index = 0; index < preBuiltSource.Count; index++)
        {
            if (index + 1 == preBuiltSource.Count)
            {
                continue;
            }
            PrioritizeIntersectedMultiTokenFromSource(preBuiltSource, expectedResult, index);
        }
    }
    
    private static void PrioritizeIntersectedMultiTokenFromSource(List<string> preBuiltSource, List<Token> expectedResult, int index)
    {
        var currentTokenSymbol = preBuiltSource[index];
        var adjacentTokenSymbol = preBuiltSource[index + 1];
        PrioritizeIntersectedMultiToken(currentTokenSymbol, adjacentTokenSymbol, expectedResult, index);
    }

    private static void PrioritizeIntersectedMultiToken(string currentSymbol, string nextSymbol, List<Token> expectedResult, int index)
    {
        var prioritizedMultiToken = GetIntersectedMultiToken(currentSymbol, nextSymbol);
        if (prioritizedMultiToken != null)
        {
            expectedResult[index] = prioritizedMultiToken;
            SetSingleTokenFromAdjacentTokenSymbol(nextSymbol, expectedResult, index);
        }
    }

    private static void SetSingleTokenFromAdjacentTokenSymbol(string adjacentSymbol, List<Token> expectedResult, int index)
    {
        var singleLexeme = adjacentSymbol[1].ToString();
        var singleTokenType = ExpectedSingleTokenSymbolTypeMappings[singleLexeme];
        expectedResult[index + 1] = new Token(singleTokenType, singleLexeme , null!, 1);
    }

    private static Token? GetIntersectedMultiToken(string tokenSymbol, string adjacentTokenSymbol)
    {
        Token? prioritizedMultiToken = null;
        
        var intersectingSymbol = $"{tokenSymbol}{adjacentTokenSymbol[0]}";
        if (ExpectedMultiTokenSymbolTypeMappings.TryGetValue(intersectingSymbol, out var prioritizedMultiTokenType))
        {
            prioritizedMultiToken = new Token(prioritizedMultiTokenType, intersectingSymbol, null!, 1);
        }

        return prioritizedMultiToken;
    }
}