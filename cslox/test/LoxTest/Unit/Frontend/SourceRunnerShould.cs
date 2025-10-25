using FluentAssertions;
using Frontend;
using Interfaces.Frontend;
using Moq;
using NUnit.Framework;

namespace Unit.Frontend;

[TestFixture]
public class SourceRunnerShould : SourceRunnerTestFixture
{
    [Test]
    public void Implement_ISourceRunner()
    {
        // Arrange
        // Act
        // Assert
        typeof(SourceRunner).Should().Implement<ISourceRunner>();
    }

    [TestCase(false, false, false)]
    [TestCase(true, false, false)]
    [TestCase(false, true, false)]
    [TestCase(false, false, true)]
    public void Run(bool hasNoTokens, bool hasOneToken, bool hasManyTokens)
    {
        // Arrange
        var consoleOutStringWriter = new StringWriter(); 
        Console.SetOut(consoleOutStringWriter);
        
        ArrangeTokens(hasNoTokens, hasOneToken, hasManyTokens);
        var expectedStandardOutput = Tokens.Aggregate("", (accumulator, current) => $"{accumulator}{current}\n");
        string? resultingStandardOutput = null;
        
        // Act (define)
        var run = () => SourceRunner.Run(Source);

        // Assert
        run.Should().NotThrow();
        resultingStandardOutput = consoleOutStringWriter.ToString();
        Console.SetOut(Console.Out);
        
        resultingStandardOutput.Should().Be(expectedStandardOutput);
        MockLexer.VerifySet(lexer => lexer.Source = Source, Times.Once);
        MockLexer.Verify(lexer => lexer.ReadTokens(),Times.Once);
        MockLexer.VerifyNoOtherCalls();
    }
}