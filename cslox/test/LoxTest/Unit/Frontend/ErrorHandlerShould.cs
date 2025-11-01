using System.Runtime.CompilerServices;
using FluentAssertions;
using Frontend;
using Interfaces.Frontend;
using NUnit.Framework;

namespace Unit.Frontend;

[TestFixture]
public class ErrorHandlerShould : ErrorHandlerTestFixture
{
    [Test, Combinatorial]
    public void Error([Values(-1, 0, 1, 2)] int line, [Values(null, "", " ", "message")] string message)
    {
        // Arrange
        Line = line;
        Message = message == "message" ? Create<string>() : message;
        
        var consoleErrorStringWriter = new StringWriter();
        Console.SetError(consoleErrorStringWriter);
        var expectedStandardErrorOutput = $"[line {(Line < 0 ? 0 : Line)}] Error: {Message}\n";
        string? resultingStandardErrorOutput = null;
        
        // Act (define)
        var error = () => ErrorHandler.Error(Line, Message);

        // Assert
        error.Should().NotThrow();
        resultingStandardErrorOutput = consoleErrorStringWriter.ToString();
        Console.SetError(Console.Error);
        resultingStandardErrorOutput.Should().Be(expectedStandardErrorOutput);
    }
    
    [Test]
    public void Implement_IErrorHandler()
    {
        // Arrange
        // Act
        // Assert
        typeof(ErrorHandler).Should().Implement<IErrorHandler>();
    }
}