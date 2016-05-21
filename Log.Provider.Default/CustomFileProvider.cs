using Log.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log.Provider.Default
{
    public class CustomFileProvider : ILogProvider
    {
        public string Name { get; private set; }
        public string Path { get; private set; }

        private readonly string _fullPath;

        public CustomFileProvider(string path, string name)
        {
            Name = name;
            Path = path;

            if (Directory.Exists(Path))
            {
                var files = Directory.GetFiles(Path, Name + "*");
                var info = from f in files
                           where f.ToLower().EndsWith(".log") || f.ToLower().EndsWith(".txt")
                           select new FileInfo(f);

                _fullPath = info.OrderBy(x => x.LastWriteTimeUtc).FirstOrDefault().FullName;
            }
        }

        public async Task<IEnumerable<LogEntry>> GetEntries()
        {
            var logItem = new LogItem();
            logItem.Name = Name;

            if(!string.IsNullOrEmpty(_fullPath))
            {
                using(var stream = File.OpenText(_fullPath))
                {
                    var json = await stream.ReadLineAsync();
                    while(!string.IsNullOrEmpty(json))
                    {
                        logItem.Entries.Add(JsonConvert.DeserializeObject<LogEntry>(json));
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

            if (!string.IsNullOrEmpty(_fullPath))
            {
                using (var stream = File.OpenText(_fullPath))
                {
                    var json = await stream.ReadLineAsync();
                    while (!string.IsNullOrEmpty(json))
                    {
                        var entry = JsonConvert.DeserializeObject<LogEntry>(json);
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
