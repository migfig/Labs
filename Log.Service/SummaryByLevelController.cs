using Log.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Log.Service
{
    public class SummaryByLevelController : ApiController
    {
        private readonly ILogServices _logServices;  
        
        //todo: need to propery configure IoC resolver
        public SummaryByLevelController()
        {
            _logServices = (LogService.Container.Resolve<ILogServices>());
        }      

        public SummaryByLevelController(ILogServices logServices)
        {
            _logServices = logServices;
        }


        [Route("api/summarybylevel"), HttpGet]
        public async Task<IEnumerable<LogSummaryByLevel>> GetSummaryByLevel()
        {
            var groupedEntries = (await _logServices.GetEntries())
                .GroupBy(x => x.EventLevel);

            return (groupedEntries.Select(group =>
                new LogSummaryByLevel
                {
                    EventLevel = group.Key.ToString(),
                    Count = group.Count()
                })).OrderByDescending(x => x.Count);
        }

        [Route("api/summarybylevel/{timeSpan}"), HttpGet]
        public async Task<IEnumerable<LogSummaryByLevel>> GetSummaryByLevel(int timeSpan)
        {
            IEnumerable<IGrouping<eEventLevel, LogEntry>> groupedEntries = null;
            if (timeSpan == 0)
            {
                groupedEntries = (await _logServices.GetEntries())
                    .GroupBy(x => x.EventLevel);
            }
            else
            {
                groupedEntries = (await _logServices.GetEntries())
                    .Where(x => DateTime.UtcNow.Subtract(x.TimeStamp).Minutes <= timeSpan)
                    .GroupBy(x => x.EventLevel);
            }

            return (groupedEntries.Select(group =>
                new LogSummaryByLevel
                {
                    EventLevel = group.Key.ToString(),
                    Count = group.Count()
                })).OrderByDescending(x => x.Count);
        }      
    }
}
