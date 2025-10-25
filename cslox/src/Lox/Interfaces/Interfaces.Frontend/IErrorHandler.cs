namespace Interfaces.Frontend;

public interface IErrorHandler
{
    void Error(int line, string message);
}