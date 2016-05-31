using Log.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Log.Service
{
    public interface ILogServices
    {
        Task<IEnumerable<LogEntry>> GetEntries();
        Task<IEnumerable<LogEntry>> GetEntries(TimeSpan span);
    }

    public class LogServices: ILogServices
    {
        private readonly ILogProvider _logProvider;
        private LogItem _logItem;
        private int _maxItems;

        public LogServices(ILogProvider logProvider, int maxItems)
        {
            _logProvider = logProvider;
            _maxItems = maxItems;
        }

        public async Task<IEnumerable<LogEntry>> GetEntries()
        {
            _logItem = new LogItem { Entries = (await _logProvider.GetEntries()).ToList() };

            return _logItem.Entries
                .OrderByDescending(x => x.TimeStamp)
                .Take(_maxItems);
        }

        public async Task<IEnumerable<LogEntry>> GetEntries(TimeSpan span)
        {
            var entries = await _logProvider.GetEntries(span);
            if (_logItem == null)
            {
                _logItem = new LogItem { Entries = entries.ToList() };
            }
            else
            {
                _logItem.Entries.Concat(entries);
            }

            return _logItem.Entries
                .OrderByDescending(x => x.TimeStamp)
                .Take(_maxItems);
        }
    }
}
