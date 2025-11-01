namespace Syntax;

public class Token(TokenType type, string lexeme, object literal, int line)
{
    public override string ToString() => throw new NotImplementedException();
}