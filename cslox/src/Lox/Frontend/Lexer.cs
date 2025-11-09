using Interfaces.Frontend;
using Syntax;

namespace Frontend;

public class Lexer(IErrorHandler errorHandler) : ILexer
{
    public IEnumerable<Token> ReadTokens(string source)
    {
        throw new NotImplementedException();
    }
}