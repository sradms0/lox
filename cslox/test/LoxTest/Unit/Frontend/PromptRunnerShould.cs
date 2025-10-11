using FluentAssertions;
using Frontend;
using Interfaces.Frontend;
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
}