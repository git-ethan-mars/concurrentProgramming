using System.Diagnostics;
using System.Text;

namespace Homework1
{
    class Program
    {
        public static void Main()
        {
            var process = Process.GetCurrentProcess();
            var data = new List<Tuple<int, long>>();
            process.ProcessorAffinity = (IntPtr) Math.Pow(2, Environment.ProcessorCount - 1);
            process.PriorityClass = ProcessPriorityClass.RealTime;
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var thread = new Thread(() =>
            {
                for (var i = 0; i < 100000; i++)
                {
                    var str = new StringBuilder("a");
                    for (var j = 0; j < 100; j++)
                        str.Append('a');
                    data.Add(Tuple.Create(1, stopwatch.ElapsedMilliseconds));
                }
            });
            var thread2 = new Thread(() =>
            {
                for (var i = 0; i < 100000; i++)
                {
                    var str = new StringBuilder("a");
                    for (var j = 0; j < 100; j++)
                        str.Append('a');
                    data.Add(Tuple.Create(2, stopwatch.ElapsedMilliseconds));
                }
            });
            thread.Start();
            thread2.Start();
            thread.Join();
            thread2.Join();
            var count = 0;
            long sum = 0;
            long temp = 0;
            var threadNum = 1;
            foreach (var item in data)
            {
                if (item.Item1 != threadNum)
                {
                    threadNum = item.Item1;
                    count++;
                    sum += (item.Item2 - temp);
                    temp = item.Item2;
                }
            }

            // ReSharper disable once IntDivisionByZero
            Console.WriteLine(sum / count);
        }
    }
}