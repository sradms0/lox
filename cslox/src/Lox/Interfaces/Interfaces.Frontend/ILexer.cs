using Syntax;

namespace Interfaces.Frontend;

public interface ILexer
{
    IEnumerable<Token> ReadTokens(string source);
}