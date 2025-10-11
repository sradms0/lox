using Interfaces.Frontend;

namespace Frontend;

public class Executor(IExitHandler exitHandler, IPromptRunner promptRunner, ISourceRunner sourceRunner) : IExecutor
{
    public void Execute(string[] args)
    {
        if (args.Length > 1)
        {
            Console.WriteLine("Usage: cslox [script]");
            const int InvalidUsageCode = 64;
            exitHandler.Exit(InvalidUsageCode);
        }
        else if (args.Length == 1)
        {
            sourceRunner.Run(args[0]);
        }
        else
        {
            promptRunner.Run();
        }
    }
}