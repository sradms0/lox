using NUnit.Framework;
using Shared;
using Syntax;

namespace Unit.Syntax;

public abstract class TokenTestFixture : CommonTestBase
{
    protected TokenType Type { get; private set; }
    
    protected string Lexeme { get; private set; } = null!;
    
    protected object Literal { get; private set; } = null!;

    private int _line;

    protected Token Token { get; private set; } = null!;
    
    [SetUp]
    public void SetUp()
    {
        Type = Create<TokenType>();

        Lexeme = Create<string>();
        
        Literal = Create<object>();
        
        _line = Create<int>();
        
        Token = new Token(Type, Lexeme, Literal, _line);
    }
}