using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Log.Common
{
    public enum eEventLevel
    {
        Information,
        Warning,
        Error,
        Critical
    }

    public class LogEntry
    {
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public eEventLevel EventLevel { get; set; }
        public int EventId { get; set; }
        public string User { get; set; }
        public string Computer { get; set; }
        public string ClassName { get; set; }
        public int LineNumber { get; set; }
    }

    public class LogItem
    {
        public string Name { get; set; }
        public IList<LogEntry> Entries { get; set; }
        public LogItem()
        {
            Entries = new List<LogEntry>();
        }
    }
}
