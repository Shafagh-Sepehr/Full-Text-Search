using FullTextSearch.IO.Abstractions;

namespace FullTextSearch.IO.Services;

public sealed class ConsoleOutput : IOutput
{
    public void Write(string text)
    {
        Console.Write(text);
    }
    
    public void WriteLine(string text)
    {
        Console.WriteLine(text);
    }
}
