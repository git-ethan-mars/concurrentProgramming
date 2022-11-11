namespace Homework2;

public static class Program
{
    public static void Main()
    {
        var multiLock = new MultiLock();
        var testVariable = new[]{"1","4"};
        var testVariable2 = new[]{"1","2","3"};
        var thread1 = new Thread(() =>
        {
            using (multiLock.AcquireLock(testVariable))
            {
                Thread.Sleep(1000);
            }
            
        });
        var thread2 = new Thread(() =>
        {
            Thread.Sleep(500);
            using(multiLock.AcquireLock(testVariable2))
            {
                Thread.Sleep(500);
            }
            
        });
        thread1.Start();
        thread2.Start();
    }
}

