using Frontend;
using Interfaces.Frontend;
using Moq;
using NUnit.Framework;
using Shared;

namespace Unit.Frontend;

public abstract class ExecutorTestFixture : CommonTestBase
{
    protected string[] Args { get; set; } = null!;
    
    protected Mock<IExitHandler> MockExitHandler { get; private set; } = null!;
    
    protected Mock<IPromptRunner> MockPromptRunner { get; private set; } = null!;
    
    protected Mock<ISourceRunner> MockSourceRunner { get; private set; } = null!;
    
    protected Executor Executor { get; private set; } = null!;

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