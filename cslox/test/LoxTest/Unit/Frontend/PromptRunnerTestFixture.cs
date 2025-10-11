using Frontend;
using Interfaces.Frontend;
using Moq;
using NUnit.Framework;
using Shared;

namespace Unit.Frontend;

public abstract class PromptRunnerTestFixture : CommonTestBase
{
    protected Mock<IInput> MockInput { get; set; }
    
    protected Mock<ISourceRunner> MockSourceRunner { get; set; }
    
    protected PromptRunner PromptRunner { get; set; }
    
    [SetUp]
    public void SetUp()
    {
        MockInput = new Mock<IInput>();
        
        MockSourceRunner = new Mock<ISourceRunner>();
        
        PromptRunner = new PromptRunner(MockInput.Object, MockSourceRunner.Object);
    }
}