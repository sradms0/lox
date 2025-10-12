using Frontend;

namespace Integration.ExitHandlerExecutable;

public static class ExitHandlerExecutable
{
    public static void Main(string[] args)
    {
        var exitCode = int.Parse(args[0]);
        new ExitHandler().Exit(exitCode);
    }
}
