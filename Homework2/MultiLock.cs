namespace Homework2;

public class MultiLock : IMultiLock
{
    private static Dictionary<string, object> ObjectByKey = new();
    private string[] currentKeys;

    public IDisposable AcquireLock(params string[] keys)
    {
        foreach (var key in keys)
        {
            currentKeys = keys;
            if (!ObjectByKey.ContainsKey(key))
            {
                ObjectByKey[key] = new();
            }
        }

        foreach (var key in keys)
            Monitor.Enter(ObjectByKey[key]);


        return this;
    }

    public void Dispose()
    {
        foreach (var key in currentKeys)
        {
            Console.WriteLine(key);
            Monitor.Exit(ObjectByKey[key]);
        }
    }
}