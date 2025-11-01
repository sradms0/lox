using Interfaces.Frontend;

namespace Frontend;

public class ErrorHandler : IErrorHandler
{
    public void Error(int line, string message) => Report(line, "", message);

    private static void Report(int line, string where, string message)
    {
        var reportMessage = $"[line {(line < 0 ? 0 : line)}] Error{where}: {message}";
        Console.Error.WriteLine(reportMessage);
    }
}