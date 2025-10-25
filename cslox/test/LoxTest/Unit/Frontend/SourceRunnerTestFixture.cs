using Frontend;
using Interfaces.Frontend;
using Moq;
using NUnit.Framework;
using Shared;
using Syntax;

namespace Unit.Frontend;

public abstract class SourceRunnerTestFixture : CommonTestBase
{
    protected string Source { get; set; }
    
    protected List<Token> Tokens { get; set; }
    
    protected Mock<ILexer> MockLexer { get; set; }
    
    protected SourceRunner SourceRunner { get; set; }

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