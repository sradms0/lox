using Syntax;

namespace Interfaces.Frontend;

public interface ILexer
{
    string Source { set; }

    IEnumerable<Token> ReadTokens();
}