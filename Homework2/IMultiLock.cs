namespace Homework2;

public interface IMultiLock 
{
    public IDisposable AcquireLock(params string[] keys);
}