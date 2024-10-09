namespace FullTextSearch.Exceptions;

public class NullInputException: Exception
{
    public override string Message => "The input text is null or white spaces";
}
