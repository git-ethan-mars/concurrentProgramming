namespace Homework2;

public static class Program
{
    public static void Main()
    {
        var testVariable = new[]{"foo","bar","z"};
        var testVariable2 = new[] {"foo"};
        var testVariable3 = new[] {"bar", "z"};
        var multilock = new MultiLock();
        var thread1 = new Thread(() =>
        {
            using (multilock.AcquireLock(testVariable))
            {
                Thread.Sleep(1000);
            }
            
        });
        var thread2 = new Thread(() =>
        {
            Thread.Sleep(500);
            using(multilock.AcquireLock(testVariable2))
            {
                Thread.Sleep(500);
                using (multilock.AcquireLock(testVariable3))
                {
                    
                }
            }
        });
        thread1.Start();
        thread2.Start();
    }
}

