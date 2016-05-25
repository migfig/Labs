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

        [Route("api/summary/{timeSpan}"), HttpGet]
        public async Task<IEnumerable<IGrouping<eEventLevel, LogEntry>>> GetSummary(int timeSpan)
        {
            if (timeSpan == 0)
            {
                return (await _logServices.GetEntries())
                    .GroupBy(x => x.EventLevel);
            }
            else
            {
                return (await _logServices.GetEntries())
                    .Where(x => DateTime.UtcNow.Subtract(x.TimeStamp).Minutes <= timeSpan)
                    .GroupBy(x => x.EventLevel);
            }
        }
    }
}
