namespace Homework2;

public static class MultiLock
{
    private static readonly Dictionary<string, object> ObjectByKey = new();

    public static IDisposable AcquireLock(params string[] keys)
    {
        foreach (var key in keys)
        {
            if (!ObjectByKey.ContainsKey(key))
            {
                ObjectByKey[key] = new object();
            }
        }

        foreach (var key in keys)
            Monitor.Enter(ObjectByKey[key]);


        return new Disposable(keys.Select(key => ObjectByKey[key]).ToArray());
    }
}