using System.Threading.Tasks;
using System.Web.Http;
using Interviewer.Common;
using Interviewer.Data.Repositories;
using System.Configuration;
using Newtonsoft.Json;
using System.Web.Http.Results;

namespace Interviewer.Services.Web.Controllers
{
    public class PlatformsController : ApiController
    {
        private IRepository _repository;
        public PlatformsController()
        {
            _repository = new InterviewerRepository(ConfigurationManager.AppSettings["InterviewerDbConnection"]);
        }

        // GET: api/Configuration
        [Route("api/configuration"), HttpGet]
        public async Task<JsonResult<configuration>> GetConfiguration()
        {
            var config = await _repository.GetConfiguration();
            //var result = JsonConvert.SerializeObject(config);
            return Json(config);

            //return result;
        }
    }
}