using Interfaces.Frontend;

namespace Frontend;

public class PromptRunner(IInput input, ISourceRunner sourceRunner) : IPromptRunner
{
    public void Run()
    {
        throw new NotImplementedException();
    }
}