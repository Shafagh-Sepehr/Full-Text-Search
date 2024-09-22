namespace CodeStar2.Interfaces;

public interface IInvertedIndexDictionary
{
    IEnumerable<string> Search(string query);
}
