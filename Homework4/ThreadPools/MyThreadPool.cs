using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace CustomThreadPool
{
    public class MyThreadPool : IThreadPool
    {
        private long _tasksProcessedCount;
        private readonly Queue<Action> _queue;
        private readonly int _maxThreadsCount;
        private readonly Thread[] _threads;


        public MyThreadPool()
        {
            _queue = new Queue<Action>();
            _maxThreadsCount = Environment.ProcessorCount * 2;
            _threads = new Thread[_maxThreadsCount];
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
                _threads[i] = thread;
                thread.Start();
            }
        }

        private void WorkingThread()
        {
            while (true)
            {
                lock (_queue)
                {
                    if (_queue.TryDequeue(out var action))
                    {
                        action();
                        Interlocked.Increment(ref _tasksProcessedCount);
                        
                    }
                    else
                    {
                        Monitor.Wait(_queue);
                    }
                }
            }
        }

        public void EnqueueAction(Action action)
        {
            lock (_queue)
            {
                _queue.Enqueue(action);
                Monitor.Pulse(_queue);
            }
        }

        public long GetTasksProcessedCount()
        {
            return _tasksProcessedCount;
        }
    }
}