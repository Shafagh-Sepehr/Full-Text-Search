namespace FullTextSearch.Exceptions;

public class NullInputException: Exception
{
    
    public NullInputException() : base("The input text is null or white spaces") //Default message
    {
        
    }

    public NullInputException(string message) : base(message)
    {
        
    }

    public NullInputException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}
