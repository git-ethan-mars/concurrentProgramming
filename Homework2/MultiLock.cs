namespace Homework2;

public class MultiLock : IMultiLock
{
    private static Dictionary<string, object> ObjectByKey = new();

    public IDisposable AcquireLock(params string[] keys)
    {
        foreach (var key in keys)
        {
            if (!ObjectByKey.ContainsKey(key))
            {
                ObjectByKey[key] = new();
            }
        }

        foreach (var key in keys)
            Monitor.Enter(ObjectByKey[key]);


        return new Disposable(keys.Select(key => ObjectByKey[key]).ToArray());
    }
}