using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Log.Common
{
    public enum eEventLevel
    {
        Information,
        Warning,
        Error,
        Critical,
        All
    }

    public class LogEntry
    {
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }
        public string ShortMessage
        {
            get { return Message.Substring(0, Math.Min(Message.Length, 50)); }
        }
        public string Source { get; set; }
        public eEventLevel EventLevel { get; set; }
        public int EventId { get; set; }
        public string User { get; set; }
        public string Computer { get; set; }

        private string _className;
        public string ClassName {
            get
            {
                if(string.IsNullOrWhiteSpace(_className) && Message.ToLower().Contains("exception"))
                {
                    var re = new Regex(@"(?<class>[a-zA-Z0-9\\\.]*.cs) line number: (?<line>\d{1,9})");
                    var match = re.Match(Message);
                    if (match.Success)
                    {
                        _className = match.Groups["class"].Value;
                        var line = 0;
                        if (int.TryParse(match.Groups["line"].Value, out line))
                        {
                            LineNumber = line;
                        }
                    }
                }

                return _className;
            }
            set { _className = value; }
        }

        public int LineNumber { get; set; }
        public string Color
        {
            get
            {
                return EventLevel.Equals(eEventLevel.Error)
                  ? "#FFB41A1E"
                  : EventLevel.Equals(eEventLevel.Warning)
                      ? "#FFFFCE42"
                      : EventLevel.Equals(eEventLevel.Information)
                          ? "#FF599737"
                          : "#FF000000";
            }
        }
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
