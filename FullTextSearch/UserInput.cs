using FullTextSearch.Exceptions;

namespace FullTextSearch;

public static class UserInput
{
   public static string GetUserQueryFromConsole()
   {
      string? query = Console.ReadLine();

      if (string.IsNullOrWhiteSpace(query)) throw new NullInputException();

      return query;
   }
}
