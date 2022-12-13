using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Homework5.LogParsers
{
    public class ParallelLogParser : ILogParser
    {
        private readonly FileInfo _file;
        private readonly Func<string, string> _tryGetIdFromLine;
        public ParallelLogParser(FileInfo file, Func<string, string> tryGetIdFromLine)
        {
            _file = file;
            _tryGetIdFromLine = tryGetIdFromLine;
        }

        public string[] GetRequestedIdsFromLogFile()
        {
            var result = new ConcurrentBag<string>();
            var lines = File.ReadLines(_file.FullName);
            Parallel.ForEach(lines, (line) => result.Add(_tryGetIdFromLine(line)));
            return result.Where(line => line is not null).ToArray();
        }
    }
}