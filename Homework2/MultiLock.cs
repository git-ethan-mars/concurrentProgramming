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

        bool lockTaken;
        var sortedKeys = keys.OrderBy(k => k).ToArray();
        try
        {

            foreach (var key in sortedKeys)
            {
                lockTaken = false;
                Monitor.Enter(ObjectByKey[key], ref lockTaken);
            }

            return new Disposable(sortedKeys.Reverse().Select(key => ObjectByKey[key]).ToArray());
        }
        catch (Exception)
        {
            foreach (var key in  sortedKeys.Reverse())
            {
                if (Monitor.IsEntered(ObjectByKey[key]))
                {
                    Monitor.Exit(key);
                }
            }
            throw;
        }
    }

}