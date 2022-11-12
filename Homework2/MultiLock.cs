namespace Homework2;

public class MultiLock : IMultiLock
{
    public IDisposable AcquireLock(params string[] obj)
    {
        return new DisposableClass(obj);
    }
}