using System;
using System.Collections.Concurrent;
using System.Threading;

namespace CustomThreadPool.ThreadPools
{
    public class MyThreadPool : IThreadPool
    {
        private long _tasksProcessedCount;
        private readonly ConcurrentQueue<Action> _queue;
        private readonly int _maxThreadsCount;


        public MyThreadPool()
        {
            _queue = new ConcurrentQueue<Action>();
            _maxThreadsCount = Environment.ProcessorCount * 2;
            Initialize();
        }

        private void Initialize()
        {
            for (var i = 0; i < _maxThreadsCount; i++)
            {
                var thread = new Thread(WorkingThread)
                {
                    IsBackground = true,
                };
                thread.Start();
            }
        }

        private void WorkingThread()
        {
            while (true)
            {
                if (_queue.TryDequeue(out var action))
                {
                    action();
                    Interlocked.Increment(ref _tasksProcessedCount);
                }
                else
                {
                    lock (_queue)
                        Monitor.Wait(_queue);
                }
            }
        }

        public void EnqueueAction(Action action)
        {
            _queue.Enqueue(action);
            lock(_queue)
                Monitor.Pulse(_queue);
        }

        public long GetTasksProcessedCount()
        {
            return _tasksProcessedCount;
        }
    }
}