using Interfaces.Frontend;

namespace Frontend;

public class ExitHandler : IExitHandler
{
    public void Exit(int exitCode) => Environment.Exit(exitCode);
}