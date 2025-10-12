using Interfaces.Frontend;

namespace Frontend;

public class Input(TextReader texReader) : IInput
{
    public string? ReadLine() => texReader.ReadLine();
}