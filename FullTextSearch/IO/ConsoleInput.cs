using FullTextSearch.Exceptions;
using FullTextSearch.IO.Interfaces;

namespace FullTextSearch.IO;

public class ConsoleInput : IInput
{
   public string ReadLine()
   {
      string? query = Console.ReadLine();

      if (string.IsNullOrWhiteSpace(query)) throw new NullInputException();

      return query;
   }
}
