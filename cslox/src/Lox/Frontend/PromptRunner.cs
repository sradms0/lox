using Interfaces.Frontend;

namespace Frontend;

public class PromptRunner(IInput input, ISourceRunner sourceRunner) : IPromptRunner
{
    public void Run()
    {
        for (;;)
        {
            Console.Write("> ");
            var lineRead = input.ReadLine();
            if (lineRead == null)
            {
                break;
            }
            
            sourceRunner.Run(lineRead);
        }
    }
}