using FullTextSearch.Exceptions;

namespace FullTextSearch.IO;

public class ConsoleInput : IInput
{
   public string ReadLine()
   {
      var query = Console.ReadLine();

      if (string.IsNullOrWhiteSpace(query)) throw new NullInputException();

      return query;
   }
}
