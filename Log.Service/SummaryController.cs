using Log.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Log.Service
{
    public class SummaryController: ApiController
    {
        private readonly ILogServices _logServices;  
        
        //todo: need to propery configure IoC resolver
        public SummaryController()
        {
            _logServices = (LogService.Container.Resolve<ILogServices>());
        }      

        public SummaryController(ILogServices logServices)
        {
            _logServices = logServices;
        }

        [Route("api/summary"), HttpGet]
        public async Task<IEnumerable<LogSummary>> GetSummary()
        {
            var groupedEntries = (await _logServices.GetEntries())
                .GroupBy(x => x.ShortMessage);

            return (groupedEntries.Select(group =>
                new LogSummary
                {
                    ShortMessage = group.Key,
                    Count = group.Count()
                })).OrderByDescending(x => x.Count);
        }

        [Route("api/summary/{timeSpan}"), HttpGet]
        public async Task<IEnumerable<LogSummary>> GetSummary(int timeSpan)
        {
            IEnumerable<IGrouping<string, LogEntry>> groupedEntries = null;
            if (timeSpan == 0)
            {
                groupedEntries = (await _logServices.GetEntries())
                    .GroupBy(x => x.ShortMessage);
            }
            else
            {
                groupedEntries = (await _logServices.GetEntries())
                    .Where(x => DateTime.UtcNow.Subtract(x.TimeStamp).Minutes <= timeSpan)
                    .GroupBy(x => x.ShortMessage);
            }

            return (groupedEntries.Select(group =>
                new LogSummary
                {
                    ShortMessage = group.Key,
                    Count = group.Count()
                })).OrderByDescending(x => x.Count);
        }
    }
}
