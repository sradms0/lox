using FluentAssertions;
using Frontend;
using Interfaces.Frontend;
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
}