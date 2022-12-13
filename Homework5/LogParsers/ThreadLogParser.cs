using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Homework5.LogParsers
{
    public class ThreadLogParser : ILogParser
    {
        private readonly FileInfo _file;
        private readonly Func<string, string> _tryGetIdFromLine;
        private ConcurrentQueue<string> _queue;
        private readonly ConcurrentBag<string> _result = new();

        public ThreadLogParser(FileInfo file, Func<string, string> tryGetIdFromLine)
        {
            _file = file;
            _tryGetIdFromLine = tryGetIdFromLine;
        }

        private void Execute()
        {
            while (_queue.TryDequeue(out var line))
            {
                var id = _tryGetIdFromLine(line);
                if (id is not null)
                {
                    _result.Add(id);
                }
            }
        }

        public string[] GetRequestedIdsFromLogFile()
        {
            var lines = File.ReadLines(_file.FullName);
            _queue = new ConcurrentQueue<string>(lines);
            var threads = new List<Thread>();
            for (var i = 0; i < Environment.ProcessorCount * 2; i++)
            {
                threads.Add(new Thread(Execute));
                threads[i].Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            return _result.ToArray();
        }
    }
}