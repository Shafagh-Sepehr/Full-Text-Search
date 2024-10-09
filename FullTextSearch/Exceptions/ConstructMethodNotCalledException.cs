namespace FullTextSearch.Exceptions;

public class ConstructMethodNotCalledException : Exception
{
    public override string Message { get; } = "The Construct Method is not called";
}
