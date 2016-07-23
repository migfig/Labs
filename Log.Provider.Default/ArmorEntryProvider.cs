using System;
using Log.Common;
using Newtonsoft.Json;

namespace Log.Provider.Default
{
    public class CustomEntryProvider : IEntryProvider
    {
        public LogEntry GetEntry(string line)
        {
            var entry = JsonConvert.DeserializeObject<CustomLogEntry>(line);
            return new LogEntry
            {
                TimeStamp = entry.Timestamp,
                EventLevel = (eEventLevel)Enum.Parse(typeof(eEventLevel), entry.Level),
                Message = entry.MessageTemplate + " " + entry.Exception ?? string.Empty,
                Source = entry.Properties.LOG,
                Computer = entry.Properties.MachineName,
                User = entry.Properties.ApplicationIdentity
            };
        }
    }

    public class CustomLogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Level { get; set; }
        public string MessageTemplate { get; set; }
        public string Exception { get; set; }
        public Properties Properties { get; set; }
    }

    public class Properties
    {
        public string LOG { get; set; }
        public string ServiceModule { get; set; }
        public string MachineName { get; set; }
        public string ApplicationIdentity { get; set; }
        public string ThreadIdentity { get; set; }
    }
}
