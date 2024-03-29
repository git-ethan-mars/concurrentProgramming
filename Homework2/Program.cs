﻿using System.Transactions;

namespace Homework2;

public static class Program
{
    public static void Main()
    {
        var testVariable = new[]{"key1","key2"};
        var testVariable2 = new[] {"key2", "key1"};
        var thread1 = new Thread(() =>
        {
            using (MultiLock.AcquireLock(testVariable))
            {
                Console.WriteLine("1 LOCKED");
                Thread.Sleep(1000);
            }
            Console.WriteLine("1 UNLOCKED");

            
            
        });
        var thread2 = new Thread(() =>
        {
            using (MultiLock.AcquireLock(testVariable2))
            {
                Console.WriteLine("2 LOCKED");
            }
            Console.WriteLine("2 UNLOCKED");
        });
        thread1.Start();
        thread2.Start();
        thread1.Join();
        thread2.Join();
    }
}

