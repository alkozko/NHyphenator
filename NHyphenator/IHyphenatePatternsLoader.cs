namespace NHyphenator
{
    public interface IHyphenatePatternsLoader
    {
        string LoadExceptions();
        string LoadPatterns();
    }
}