using System.Diagnostics;

namespace Homework1
{
    internal static class Program
    {
        private static int _currentThreadId = Environment.CurrentManagedThreadId;

        public static void Main()
        {
            var data = new List<long>();
            var process = Process.GetCurrentProcess();
            process.ProcessorAffinity = (IntPtr) Math.Pow(2, Environment.ProcessorCount - 1);
            process.PriorityClass = ProcessPriorityClass.RealTime;
            var startTime = DateTime.Now;
            var thread = new Thread(() =>
            {
                while ((DateTime.Now - startTime).Seconds < 1)
                {
                    if (_currentThreadId != Environment.CurrentManagedThreadId)
                    {
                        _currentThreadId = Environment.CurrentManagedThreadId;
                        data.Add(Convert.ToInt64((DateTime.Now-DateTime.UnixEpoch).TotalMilliseconds));
                    }
                }
            });
            var thread2 = new Thread(() =>
            {
                while ((DateTime.Now - startTime).Seconds < 1)
                {
                    if (_currentThreadId != Environment.CurrentManagedThreadId)
                    {
                        _currentThreadId = Environment.CurrentManagedThreadId;
                        data.Add(Convert.ToInt64((DateTime.Now-DateTime.UnixEpoch).TotalMilliseconds));
                    }
                }
            });
            thread.Start();
            thread2.Start();
            thread.Join();
            thread2.Join();
            var previousTime = data.First();
            var result = new List<long>();
            foreach (var nextTime in data.Skip(1))
            {
                result.Add(nextTime-previousTime);
                previousTime = nextTime;
            }
            Console.WriteLine(result.Average());
        }
    }
}