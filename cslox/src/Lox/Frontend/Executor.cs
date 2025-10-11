using Interfaces.Frontend;

namespace Frontend;

public class Executor(IExitHandler exitHandler, IPromptRunner promptRunner, ISourceRunner sourceRunner) : IExecutor
{
    public void Execute(string[] args)
    {
        throw new NotImplementedException();
    }
}