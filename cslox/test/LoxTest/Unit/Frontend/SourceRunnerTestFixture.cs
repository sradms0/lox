using Frontend;
using Interfaces.Frontend;
using Moq;
using NUnit.Framework;
using Shared;
using Syntax;

namespace Unit.Frontend;

public abstract class SourceRunnerTestFixture : CommonTestBase
{
    protected string Source { get; private set; } = null!;
    
    protected List<Token> Tokens { get; private set; } = null!;
    
    protected Mock<ILexer> MockLexer { get; private set; } = null!;
    
    protected SourceRunner SourceRunner { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        Source = Create<string>();

        Tokens = CreateMany<Token>().ToList();
        
        MockLexer = new Mock<ILexer>();
        
        MockLexer
            .Setup(lexer => lexer.ReadTokens())
            .Returns(() => Tokens);
        
        SourceRunner = new SourceRunner(MockLexer.Object);
    }

    protected void ArrangeTokens(bool hasNoTokens, bool hasOneToken, bool hasManyTokens)
    {
        if (hasNoTokens)
        {
            Tokens = [];
        }
        else if (hasOneToken)
        {
            Tokens = [Tokens.First()];
        }
        else if (hasManyTokens)
        {
            Tokens = Tokens[..^1];
        }
    }
}