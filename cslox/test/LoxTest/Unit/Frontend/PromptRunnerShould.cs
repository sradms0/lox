using FluentAssertions;
using Frontend;
using Interfaces.Frontend;
using Moq;
using NUnit.Framework;

namespace Unit.Frontend;

[TestFixture]
public class PromptRunnerShould : PromptRunnerTestFixture
{
    [Test]
    public void Implement_IPromptRunner()
    {
        // Arrange
        // Act (define)
        // Assert
        typeof(PromptRunner).Should().Implement<IPromptRunner>();
    }

    [TestCase(false, false)]
    [TestCase(true, false)]
    [TestCase(false, true)]
    public void Run(bool hasOneLineRead, bool hasManyLinesRead)
    {
        // Arrange
        var expectedMockInputCallCount =
            SetupMockInputReadLineAndDetermineExpectedCallCount(hasOneLineRead, hasManyLinesRead);
        
        var consoleOutStringWriter = new StringWriter(); 
        Console.SetOut(consoleOutStringWriter);

        var expectedPromptOutput = Enumerable.Range(0, expectedMockInputCallCount)
            .Select(_ => ExpectedPromptOutputString)
            .Aggregate((accumulator, current) => accumulator + current);
        
        var expectedMockSourceRunnerCallCount = expectedMockInputCallCount - 1;
        string? resultingStandardOutput = null;
        
        // Act (define)
        var run = () => PromptRunner.Run();

        // Assert
        run.Should().NotThrow();
        resultingStandardOutput = consoleOutStringWriter.ToString();
        Console.SetOut(Console.Out);
        resultingStandardOutput.Should().Be(expectedPromptOutput);
        
        MockInput.Verify(input => input.ReadLine(), Times.Exactly(expectedMockInputCallCount));
        MockInput.VerifyNoOtherCalls();
        MockSourceRunner.Verify(runner => runner.Run(LineRead), Times.Exactly(expectedMockSourceRunnerCallCount));
        MockSourceRunner.VerifyNoOtherCalls();
    }
}