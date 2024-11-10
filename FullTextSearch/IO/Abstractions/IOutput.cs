namespace FullTextSearch.IO.Abstractions;

public interface IOutput
{
    void Write(string text);
    void WriteLine(string text);
}
