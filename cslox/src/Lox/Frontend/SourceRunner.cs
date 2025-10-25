using Interfaces.Frontend;

namespace Frontend;

public class SourceRunner(ILexer lexer) : ISourceRunner
{
    public void Run(string source)
    {
        lexer.Source = source;
        foreach (var token in lexer.ReadTokens())
        {
            Console.WriteLine(token);
        }
    }
}