using Frontend;
using Interfaces.Frontend;
using Moq;
using NUnit.Framework;
using Shared;

namespace Unit.Frontend;

public abstract class PromptRunnerTestFixture : CommonTestBase
{
    protected const string ExpectedPromptOutputString = "> ";
    
    protected string LineRead { get; set; }
    
    protected Mock<IInput> MockInput { get; set; }
    
    protected Mock<ISourceRunner> MockSourceRunner { get; set; }
    
    protected PromptRunner PromptRunner { get; set; }
    
    [SetUp]
    public void SetUp()
    {
        LineRead = Create<string>();
        
        MockInput = new Mock<IInput>();
        
        MockSourceRunner = new Mock<ISourceRunner>();
        
        PromptRunner = new PromptRunner(MockInput.Object, MockSourceRunner.Object);
    }
    
    protected int SetupMockInputReadLineAndDetermineExpectedCallCount(bool hasOneLineRead, bool hasManyLinesRead)
    {
        var expectedCallOut = 1;
        var sequence = MockInput.SetupSequence(input => input.ReadLine());
        
        if (hasOneLineRead)
        {
            sequence.Returns(LineRead);
            expectedCallOut++;
        }
        else if (hasManyLinesRead)
        {
            sequence.Returns(LineRead).Returns(LineRead);
            expectedCallOut += 2;
        }
        sequence.Returns((string?)null);
        
        return expectedCallOut;
    }
}