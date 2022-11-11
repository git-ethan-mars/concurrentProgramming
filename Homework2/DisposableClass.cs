namespace Homework2;

public class DisposableClass : IDisposable
{
    private bool _lockTaken;
    private readonly HashSet<string> _keysToLock;
    private static readonly List<string> BlockedKeys = new();
    private static readonly List<(HashSet<string>, Thread)> KeysThreadPairList = new();

    public DisposableClass(string[] lockableObject)
    {
        _keysToLock = new HashSet<string>(lockableObject);
        lock (KeysThreadPairList)
            KeysThreadPairList.Add((new HashSet<string>(lockableObject), Thread.CurrentThread));
        if (AreKeysBlocked(_keysToLock))
        {
            WaitForReleaseObject();
        }
        else
        {
            AcquireObject();
        }
    }


    public void Dispose()
    {
        if (!_lockTaken) return;
        Monitor.Exit(_keysToLock);
        Console.WriteLine("Объект разблокирован");
        lock (KeysThreadPairList)
        {
            lock (BlockedKeys)
            {
                foreach (var key in _keysToLock)
                {
                    BlockedKeys.Remove(key);
                }

                foreach (var (keys, thread) in KeysThreadPairList)
                {
                    var foundNextKeysToAcquire = true;
                    foreach (var key in keys)
                    {
                        if (BlockedKeys.Contains(key))
                        {
                            foundNextKeysToAcquire = false;
                            break;
                        }
                    }

                    if (foundNextKeysToAcquire)
                    {
                        thread.Interrupt();
                        break;
                    }
                }
            }
        }
    }

    private void AcquireObject()
    {
        lock (KeysThreadPairList)
        {
            for (var i = 0; i < KeysThreadPairList.Count; i++)
            {
                if (KeysThreadPairList[i].Item1.SetEquals(_keysToLock) && KeysThreadPairList[i].Item2.ManagedThreadId ==
                    Environment.CurrentManagedThreadId)
                {
                    KeysThreadPairList.RemoveAt(i);
                    break;
                }
            }
        }

        lock (BlockedKeys)
        {
            foreach (var obj in _keysToLock)
            {
                BlockedKeys.Add(obj);
            }
        }

        _lockTaken = false;
        Monitor.Enter(_keysToLock, ref _lockTaken);
        Console.WriteLine(_lockTaken ? "Объект заблокирован" : "Объект не получилось заблокировать");
    }

    private static bool AreKeysBlocked(HashSet<string> keys)
    {
        lock (BlockedKeys)
        {
            foreach (var key in keys)
            {
                if (BlockedKeys.Contains(key))
                {
                    return true;
                }
            }

            return false;
        }
    }

    private void WaitForReleaseObject()
    {
        try
        {
            Thread.Sleep(Timeout.Infinite);
        }
        catch (ThreadInterruptedException)
        {
            AcquireObject();
        }
    }
}