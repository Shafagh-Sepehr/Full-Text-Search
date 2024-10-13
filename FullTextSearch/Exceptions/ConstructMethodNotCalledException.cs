namespace FullTextSearch.Exceptions;

public class ConstructMethodNotCalledException : Exception
{

    public ConstructMethodNotCalledException() : base("The Construct Method is not called") //Default message
    {
        
    }

    public ConstructMethodNotCalledException(string message) : base(message)
    {
        
    }

    public ConstructMethodNotCalledException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}
