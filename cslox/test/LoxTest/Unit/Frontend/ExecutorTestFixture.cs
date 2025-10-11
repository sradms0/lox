using Frontend;
using Interfaces.Frontend;
using Moq;
using NUnit.Framework;
using Shared;

namespace Unit.Frontend;

public abstract class ExecutorTestFixture : CommonTestBase
{
    protected string[] Args { get; set; } = null!;
    protected Mock<IExitHandler> MockExitHandler { get; set; }
    
    protected Mock<IPromptRunner> MockPromptRunner { get; set; }
    
    protected Mock<ISourceRunner> MockSourceRunner { get; set; }
    
    protected Executor Executor { get; set;}

    [SetUp]
    public void SetUp()
    {
        Args = [..CreateMany<string>()];
        
        MockExitHandler = new Mock<IExitHandler>();
        
        MockPromptRunner = new Mock<IPromptRunner>();
        
        MockSourceRunner = new Mock<ISourceRunner>();
        
        Executor = new Executor(MockExitHandler.Object, MockPromptRunner.Object, MockSourceRunner.Object);
    }
}