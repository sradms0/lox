using Frontend;
using NUnit.Framework;
using Shared;

namespace Unit.Frontend;

public abstract class ErrorHandlerTestFixture : CommonTestBase
{
    protected int Line { get; set; }
    
    protected string Message { get; set; } = null!;

    protected ErrorHandler ErrorHandler { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        Line = Create<int>();
        
        Message = Create<string>();
        
        ErrorHandler = new ErrorHandler();
    }
}