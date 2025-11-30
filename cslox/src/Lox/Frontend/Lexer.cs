using Interfaces.Frontend;
using Syntax;

namespace Frontend;

public class Lexer(IErrorHandler errorHandler) : ILexer
{
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

    private bool MatchAndAdvanceCurrent(char character)
    {
        var isMatched = !IsAtEndOfSource() && character == _source[_current];
        
        if (isMatched)
        {
            _current++;
        }
        
        return isMatched;
    }
    
    private void ReadToken()
    {
        var currentCharacter = AdvanceToNextCharacterInSource();
        switch (currentCharacter)
        {
            case '(':
                AddToken(TokenType.LeftParen);
                break;
            case ')':
                AddToken(TokenType.RightParen);
                break;
            case '{':
                AddToken(TokenType.LeftBrace);
                break;
            case '}':
                AddToken(TokenType.RightBrace);
                break;
            case ',':
                AddToken(TokenType.Comma);
                break;
            case '.':
                AddToken(TokenType.Dot);
                break;
            case '-':
                AddToken(TokenType.Minus);
                break;
            case '+':
                AddToken(TokenType.Plus);
                break;
            case ';':
                AddToken(TokenType.Semicolon);
                break;
            case '*':
                AddToken(TokenType.Star);
                break;
            case '!':
                AddToken(MatchAndAdvanceCurrent('=') ? TokenType.BangEqual : TokenType.Bang);
                break;
            case '=':
                AddToken(MatchAndAdvanceCurrent('=') ? TokenType.EqualEqual : TokenType.Equal);
                break;
            case '<':
                AddToken(MatchAndAdvanceCurrent('=') ? TokenType.LessEqual : TokenType.Less);
                break;
            case '>':
                AddToken(MatchAndAdvanceCurrent('=') ? TokenType.GreaterEqual : TokenType.Greater);
                break;
            case '/':
                AddToken(TokenType.Slash);
                break;
            default:
                errorHandler.Error(_line, "Unexpected Character.");
                break;
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