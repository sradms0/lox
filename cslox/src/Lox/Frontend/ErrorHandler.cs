using Interfaces.Frontend;

namespace Frontend;

public class ErrorHandler : IErrorHandler
{
    public void Error(int line, string message)
    {
        throw new NotImplementedException();
    }
}