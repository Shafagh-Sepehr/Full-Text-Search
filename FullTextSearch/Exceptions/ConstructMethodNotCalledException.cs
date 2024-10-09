namespace FullTextSearch.Exceptions;

public class ConstructMethodNotCalledException : Exception
{
    public override string Message => "The Construct Method is not called";
}
