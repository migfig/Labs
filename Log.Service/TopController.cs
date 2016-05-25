using Log.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Log.Service
{
    public class TopController: ApiController
    {
        private readonly ILogServices _logServices;  
        
        //todo: need to propery configure IoC resolver
        public TopController()
        {
            _logServices = (LogService.Container.Resolve<ILogServices>());
        }      

        public TopController(ILogServices logServices)
        {
            _logServices = logServices;
        }

        [Route("api/top/{int:count}/{level}"), HttpGet]
        public async Task<IEnumerable<LogEntry>> GetTopItems(int count, string level)
        {
            switch(level)
            {
                case "items":
                    return (await _logServices.GetEntries())
                        .OrderByDescending(x => x.TimeStamp)
                        .Take(count);
                default:
                    return (await _logServices.GetEntries())
                        .Where(x => x.EventLevel == (eEventLevel)Enum.Parse(typeof(eEventLevel), level))
                        .OrderByDescending(x => x.TimeStamp)
                        .Take(count);
            }
        }
    }
}
