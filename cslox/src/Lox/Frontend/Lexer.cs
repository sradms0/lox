using Interfaces.Frontend;
using Syntax;

namespace Frontend;

public class Lexer(IErrorHandler errorHandler) : ILexer
{
    private static readonly Dictionary<char, TokenType> CharacterTokenTypeMappings = new()
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
    
    private string _source = string.Empty;

    private int _start;

    private int _current;
    
    private int _line = 1;
    
    private readonly List<Token> _tokens = [];
    
    public IEnumerable<Token> ReadTokens(string source)
    {
        ResetMembers();
        _source = source ?? string.Empty;
        
        while (!IsAtEndOfSource())
        {
            _start = _current;
            ReadToken();
        }
        _tokens.Add(new Token(TokenType.Eof, "", null!, _line));
        
        return _tokens;
    }

    private void AddToken(TokenType tokenType) => AddToken(tokenType, null!);

    private void AddToken(TokenType tokenType, object literal)
    {
        var lexeme = _source.Substring(_start, _current - _start);
        _tokens.Add(new Token(tokenType, lexeme, literal, _line));
    }
    
    private char AdvanceToNextCharacterInSource() => _source[_current++];

    private bool IsAtEndOfSource() => _current == _source.Length;
    
    private void ReadToken()
    {
        var currentCharacter = AdvanceToNextCharacterInSource();
        if (CharacterTokenTypeMappings.TryGetValue(currentCharacter, out var token))
        {
            AddToken(token);
        }
        else
        {
            errorHandler.Error(_line, "Unexpected Character.");
        }
    }

    private void ResetMembers()
    {
        _source = string.Empty;
        _start = 0;
        _current = 0;
        _line = 1;
        _tokens.Clear();
    }
}