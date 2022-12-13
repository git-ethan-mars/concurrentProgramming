using System;
using System.IO;
using System.Linq;

namespace Homework5.LogParsers
{
    public class PLinqLogParser : ILogParser
    {
        private FileInfo _file;
        private Func<string, string> _tryGetIdFromLine;

        public PLinqLogParser(FileInfo file, Func<string, string> tryGetIdFromLine)
        {
            _file = file;
            _tryGetIdFromLine = tryGetIdFromLine;
        }

        public string[] GetRequestedIdsFromLogFile()
        {
            var lines = File.ReadLines(_file.FullName);
            return lines.AsParallel().Select(line => _tryGetIdFromLine(line))
                .Where(line => line is not null).ToArray();
        }
    }
}