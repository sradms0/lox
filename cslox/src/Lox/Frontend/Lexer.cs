using Interfaces.Frontend;
using Syntax;

namespace Frontend;

public abstract class Lexer : ILexer
{
    public string Source { private get; set; }
    
    public IEnumerable<Token> ReadTokens()
    {
        throw new NotImplementedException();
    }
}