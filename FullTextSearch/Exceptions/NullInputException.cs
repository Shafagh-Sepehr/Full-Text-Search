namespace FullTextSearch.Exceptions;

public class NullInputException: Exception
{
    public override string Message { get; } = "the input text is null or white spaces";
}
