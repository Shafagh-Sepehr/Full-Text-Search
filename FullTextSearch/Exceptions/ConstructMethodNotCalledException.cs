namespace FullTextSearch.Exceptions;

public sealed class ConstructMethodNotCalledException : Exception
{
    public ConstructMethodNotCalledException() : base("The Construct Method is not called") //Default message
    { }
    
    public ConstructMethodNotCalledException(string message) : base(message) { }
    
    public ConstructMethodNotCalledException(string message, Exception innerException) : base(message, innerException) { }
}
