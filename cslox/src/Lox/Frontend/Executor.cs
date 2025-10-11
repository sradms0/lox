using Interfaces.Frontend;

namespace Frontend;

public class Executor(IExitHandler exitHandler, IPromptRunner promptRunner, ISourceRunner sourceRunner);