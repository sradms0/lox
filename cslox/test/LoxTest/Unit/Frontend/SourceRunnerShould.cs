using FluentAssertions;
using Frontend;
using Interfaces.Frontend;
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
}