using FluentAssertions;
using Frontend;
using Interfaces.Frontend;
using Moq;
using NUnit.Framework;

namespace Unit.Frontend;

[TestFixture]
public class ExecutorShould : ExecutorTestFixture
{
    [Test]
    public void Implement_IExecutor()
    {
        // Arrange
        // Act
        // Assert
        typeof(Executor).Should().Implement<IExecutor>();
    }
    
    [Test]
    public void Execute_And_Exit_With_Output()
    {
        // Arrange
        var consoleOutStringWriter = new StringWriter();
        Console.SetOut(consoleOutStringWriter);
        
        const string ExpectedStandardOutput = "Usage: cslox [script]\n";
        string? resultingStandardOutput = null;
        const int ExpectedExitCode = 64;
            
        // Act (define)
        var execute = () => Executor.Execute(Args);

        // Assert
        execute.Should().NotThrow();
        resultingStandardOutput = consoleOutStringWriter.ToString();
        Console.SetOut(Console.Out);
        resultingStandardOutput.Should().Be(ExpectedStandardOutput);
        
        MockExitHandler.Verify(exitHandler => exitHandler.Exit(ExpectedExitCode), Times.Once);
        MockExitHandler.VerifyNoOtherCalls();
        MockPromptRunner.VerifyNoOtherCalls();
        MockSourceRunner.VerifyNoOtherCalls();
    }
    
    [Test]
    public void Execute_Source()
    {
        // Arrange
        var source = Args[0];
        Args = [source];
        
        // Act (define)
        var execute = () => Executor.Execute(Args);

        // Assert
        execute.Should().NotThrow();
        MockSourceRunner.Verify(runner => runner.Run(source), Times.Once);
        MockSourceRunner.VerifyNoOtherCalls();
        MockExitHandler.VerifyNoOtherCalls();
        MockPromptRunner.VerifyNoOtherCalls();
    }
    
    [Test]
    public void Execute_Prompt()
    {
        // Arrange
        Args = [];
        
        // Act (define)
        var execute = () => Executor.Execute(Args);

        // Assert
        execute.Should().NotThrow();
        MockPromptRunner.Verify(runner => runner.Run(), Times.Once);
        MockPromptRunner.VerifyNoOtherCalls();
        MockExitHandler.VerifyNoOtherCalls();
        MockSourceRunner.VerifyNoOtherCalls();
    }
}