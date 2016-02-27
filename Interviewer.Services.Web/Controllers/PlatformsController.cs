using System.Threading.Tasks;
using System.Web.Http;
using Interviewer.Common;
using Interviewer.Data.Repositories;
using System.Configuration;

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
        public async Task<configuration> GetConfiguration()
        {
            return await _repository.GetConfiguration();
        }
    }
}