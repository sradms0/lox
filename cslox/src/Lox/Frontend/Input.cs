using Interfaces.Frontend;

namespace Frontend;

public class Input(TextReader texReader) : IInput
{
    public string? ReadLine()
    {
        throw new NotImplementedException();
    }
}