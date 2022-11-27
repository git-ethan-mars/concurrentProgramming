namespace Homework2;

public class Disposable : IDisposable
{
    private object[] _currentKeys;
    public Disposable(object[] keys)
    {
        _currentKeys = keys;
    }
    public void Dispose()
    {
        foreach (var key in _currentKeys)
        {
            Monitor.Exit(key);
        }
    }
}