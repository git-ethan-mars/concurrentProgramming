namespace Homework5.LogParsers
{
    public interface ILogParser
    {
        string[] GetRequestedIdsFromLogFile();
    }
}