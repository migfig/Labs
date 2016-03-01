using System.Threading.Tasks;
using System.Web.Http;
using Interviewer.Common;
using Interviewer.Data.Repositories;
using System.Configuration;
using System.Web.Http.Results;
using System.Linq;

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
            return Json(config);
        }

        [Route("api/add/platform"), HttpPost]
        public async Task<int> AddPlatform([FromBody] Platform request)
        {
            return await _repository.AddItem<Platform>(request);
        }

        [Route("api/udpate/platform"), HttpPut]
        public async Task<int> UpdatePlatform([FromBody] Platform request)
        {
            return await _repository.UpdateItem<Platform>(request);
        }

        [Route("api/delete/platform/{id}"), HttpDelete]
        public async Task<int> DeletePlatform(int id)
        {
            var config = await _repository.GetConfiguration();
            var request = from p in config.Platform                          
                          where p.Id == id
                          select p;
            return await _repository.DeleteItem<Platform>(request.FirstOrDefault());
        }

        [Route("api/add/knowledgearea"), HttpPost]
        public async Task<int> AddKnowledgeArea([FromBody] KnowledgeArea request)
        {
            return await _repository.AddItem<KnowledgeArea>(request);
        }

        [Route("api/udpate/knowledgearea"), HttpPut]
        public async Task<int> UpdateKnowledgeArea([FromBody] KnowledgeArea request)
        {
            return await _repository.UpdateItem<KnowledgeArea>(request);
        }

        [Route("api/delete/knowledgearea/{id}"), HttpDelete]
        public async Task<int> DeleteKnowledgeArea(int id)
        {
            var config = await _repository.GetConfiguration();
            var request = from p in config.Platform
                          from ka in p.KnowledgeArea
                          where ka.Id == id
                          select ka;
            return await _repository.DeleteItem<KnowledgeArea>(request.FirstOrDefault());
        }

        [Route("api/add/area"), HttpPost]
        public async Task<int> AddArea([FromBody] Area request)
        {
            return await _repository.AddItem<Area>(request);
        }

        [Route("api/udpate/area"), HttpPut]
        public async Task<int> UpdateArea([FromBody] Area request)
        {
            return await _repository.UpdateItem<Area>(request);
        }

        [Route("api/delete/area/{id}"), HttpDelete]
        public async Task<int> DeleteArea(int id)
        {
            var config = await _repository.GetConfiguration();
            var request = from p in config.Platform
                          from ka in p.KnowledgeArea
                          from a in ka.Area
                          where a.Id == id
                          select a;
            return await _repository.DeleteItem<Area>(request.FirstOrDefault());
        }

        [Route("api/add/question"), HttpPost]
        public async Task<int> AddQuestion([FromBody] Question request)
        {
            return await _repository.AddItem<Question>(request);
        }

        [Route("api/udpate/question"), HttpPut]
        public async Task<int> UpdateQuestion([FromBody] Question request)
        {
            return await _repository.UpdateItem<Question>(request);
        }

        [Route("api/delete/question/{id}"), HttpDelete]
        public async Task<int> DeleteQuestion(int id)
        {
            var config = await _repository.GetConfiguration();
            var request = from p in config.Platform
                          from ka in p.KnowledgeArea
                          from a in ka.Area
                          from q in a.Question
                          where q.Id == id
                          select q;
            return await _repository.DeleteItem<Question>(request.FirstOrDefault());
        }
    }
}