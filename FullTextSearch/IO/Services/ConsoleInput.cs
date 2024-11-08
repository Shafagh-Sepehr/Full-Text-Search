using FullTextSearch.Exceptions;
using FullTextSearch.IO.Abstractions;

namespace FullTextSearch.IO.Services;

public sealed class ConsoleInput : IInput
{
    public string ReadLine()
    {
        var query = Console.ReadLine();
        
        if (string.IsNullOrWhiteSpace(query)) throw new NullInputException();
        
        return query;
    }
}
