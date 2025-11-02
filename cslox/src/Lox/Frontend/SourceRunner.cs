using Interfaces.Frontend;

namespace Frontend;

public class SourceRunner(ILexer lexer) : ISourceRunner
{
    public void Run(string source)
    {
        foreach (var token in lexer.ReadTokens(source))
        {
            Console.WriteLine(token);
        }
    }
}