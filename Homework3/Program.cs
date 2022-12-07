using System;
using System.Threading;

namespace Homework3
{
    class Program
    {
        private static Stack<int> Stack;

        static void Worker1()
        {
            for (var i = 0; i < 50000; i++)
            {
                Stack.Push(i);
            }
        }

        static void Worker2()
        {
            var i = 2000;
            while (i != 0)
                if (Stack.TryPop(out var item))
                {
                    i--;
                }
        }

        static void Main(string[] args)
        {
            Stack = new Stack<int>();
            var thread1 = new Thread(Worker1);
            var thread2 = new Thread(Worker1);
            var thread3 = new Thread(Worker2);
            thread1.Start();
            thread2.Start();
            thread3.Start();
            thread1.Join();
            thread2.Join();
            thread3.Join();
            Console.WriteLine(Stack.Count);
        }
    }
}