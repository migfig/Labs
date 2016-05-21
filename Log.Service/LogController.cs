using Log.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Log.Service
{
    public class LogController: ApiController
    {
        private readonly ILogServices _logServices;

        public LogController(ILogServices logServices)
        {
            _logServices = logServices;
        }

        [Route("api/log/summary/{span}"), HttpGet]
        public async Task<IEnumerable<IGrouping<eEventLevel, LogEntry>>> GetSummary(TimeSpan span)
        {
            return (await _logServices.GetEntries())
                .Where(x => DateTime.UtcNow.Subtract(x.TimeStamp).Seconds <= span.Seconds)
                .GroupBy(x => x.EventLevel);
        }

        [Route("api/log/items"), HttpGet]
        public async Task<IEnumerable<LogEntry>> GetItems()
        {
            return await _logServices.GetEntries();
        }

        [Route("api/log/items/{span}"), HttpGet]
        public async Task<IEnumerable<LogEntry>> GetItems(TimeSpan span)
        {
            return await _logServices.GetEntries(span);
        }

        [Route("api/log/top/errors/{int:count}"), HttpGet]
        public async Task<IEnumerable<LogEntry>> GetTopErrors(int count)
        {
            return (await _logServices.GetEntries())
                .Where(x => x.EventLevel == eEventLevel.Error)
                .OrderByDescending(x => x.TimeStamp)
                .Take(count);
        }

        [Route("api/log/top/critical/{int:count}"), HttpGet]
        public async Task<IEnumerable<LogEntry>> GetTopCritical(int count)
        {
            return (await _logServices.GetEntries())
                .Where(x => x.EventLevel == eEventLevel.Critical)
                .OrderByDescending(x => x.TimeStamp)
                .Take(count);
        }

        [Route("api/log/top/information/{int:count}"), HttpGet]
        public async Task<IEnumerable<LogEntry>> GetTopInformation(int count)
        {
            return (await _logServices.GetEntries())
                .Where(x => x.EventLevel == eEventLevel.Information)
                .OrderByDescending(x => x.TimeStamp)
                .Take(count);
        }

        [Route("api/log/top/warnings/{int:count}"), HttpGet]
        public async Task<IEnumerable<LogEntry>> GetTopWarnings(int count)
        {
            return (await _logServices.GetEntries())
                .Where(x => x.EventLevel == eEventLevel.Warning)
                .OrderByDescending(x => x.TimeStamp)
                .Take(count);
        }

        [Route("api/log/top/latest/{int:count}"), HttpGet]
        public async Task<IEnumerable<LogEntry>> GetTopLatest(int count)
        {
            return (await _logServices.GetEntries())
                .OrderByDescending(x => x.TimeStamp)
                .Take(count);
        }
    }
}
