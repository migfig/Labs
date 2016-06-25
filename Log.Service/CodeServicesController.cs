using Log.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Log.Service
{
    public class CodeServicesController: ApiController
    {
        private readonly ILogServices _logServices;  
        
        //todo: need to propery configure IoC resolver
        public CodeServicesController()
        {
            _logServices = (LogService.Container.Resolve<ILogServices>());
        }      

        public CodeServicesController(ILogServices logServices)
        {
            _logServices = logServices;
        }

        [Route("api/items"), HttpGet]
        public async Task<IEnumerable<LogEntry>> GetItems()
        {
            return await _logServices.GetEntries();
        }

        [Route("api/items/{timeSpan}"), HttpGet]
        public async Task<IEnumerable<LogEntry>> GetItems(int timeSpan)
        {
            if(timeSpan == 0)
                return await _logServices.GetEntries();
            else
                return await _logServices.GetEntries(TimeSpan.FromMinutes(timeSpan));
        }
    }
}
