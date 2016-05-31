using Log.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Log.Provider.Default
{
    public class CustomFileProvider : ILogProvider
    {
        public string Name { get; private set; }
        public string NetworkPath { get; private set; }
        public string LocalPath { get; private set; }

        private readonly string _logFilePath;
        private readonly IEntryProvider _entryProvider;

        public CustomFileProvider(string networkPath, string path, string name, IEntryProvider entryProvider)
        {
            Name = name;
            NetworkPath = networkPath;
            LocalPath = path;
            _entryProvider = entryProvider;

            if (Directory.Exists(NetworkPath))
            {
                var files = Directory.GetFiles(NetworkPath, Name.ToLower() + "*");
                var info = from f in files
                           where f.ToLower().EndsWith(".log") || f.ToLower().EndsWith(".txt")
                           select new FileInfo(f);

                var fullNetworkPath = info.OrderBy(x => x.LastWriteTimeUtc).LastOrDefault().FullName;

                if (NetworkPath != LocalPath)
                {
                    _logFilePath = Path.Combine(LocalPath, Path.GetFileName(fullNetworkPath));
                    if (File.Exists(_logFilePath))
                    {
                        File.Delete(_logFilePath);
                    }

                    File.Copy(fullNetworkPath, _logFilePath);
                }
                else
                {
                    _logFilePath = fullNetworkPath;
                }
            }
        }

        public async Task<IEnumerable<LogEntry>> GetEntries()
        {
            var logItem = new LogItem();
            logItem.Name = Name;

            if(!string.IsNullOrEmpty(_logFilePath))
            {
                using(var stream = File.OpenText(_logFilePath))
                {
                    var json = await stream.ReadLineAsync();
                    while(!string.IsNullOrEmpty(json))
                    {
                        logItem.Entries.Add(_entryProvider.GetEntry(json));
                        json = await stream.ReadLineAsync();
                    }
                }
            }

            return logItem.Entries;
        }

        public async Task<IEnumerable<LogEntry>> GetEntries(TimeSpan span)
        {
            var logItem = new LogItem();
            logItem.Name = Name;

            if (!string.IsNullOrEmpty(_logFilePath))
            {
                using (var stream = File.OpenText(_logFilePath))
                {
                    var json = await stream.ReadLineAsync();
                    while (!string.IsNullOrEmpty(json))
                    {
                        var entry = _entryProvider.GetEntry(json);
                        if (DateTime.UtcNow.Subtract(entry.TimeStamp).Seconds <= span.Seconds)
                        {
                            logItem.Entries.Add(entry);
                        }
                        json = await stream.ReadLineAsync();
                    }
                }
            }

            return logItem.Entries;
        }
    }
}
