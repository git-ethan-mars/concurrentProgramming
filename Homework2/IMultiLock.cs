namespace Homework2;

public interface IMultiLock : IDisposable
{
    public IDisposable AcquireLock(params string[] keys);
}