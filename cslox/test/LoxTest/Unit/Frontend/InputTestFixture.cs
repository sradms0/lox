using Frontend;
using Moq;
using NUnit.Framework;
using Shared;

namespace Unit.Frontend;

public abstract class InputTestFixture : CommonTestBase
{
    protected Mock<TextReader> MockTextReader { get; private set; } = null!;

    protected Input Input { get; private set; } = null!;
    
    [SetUp]
    public void SetUp()
    {
        MockTextReader = new Mock<TextReader>();
        
        Input = new Input(MockTextReader.Object);
    }
}