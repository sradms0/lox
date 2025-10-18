using Interfaces.Frontend;

namespace Frontend;

public class SourceRunner(ILexer lexer) : ISourceRunner
{
    public void Run(string source)
    {
        throw new NotImplementedException();
    }
}