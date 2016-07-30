using Common.Generics;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Trainer.Domain;

namespace Common.Controllers
{
    public class PresentationsController: ApiController
    {
        private readonly IGenericServices<Presentation> _codeServices;  
        
        public PresentationsController(IGenericServices<Presentation> codeServices)
        {
            _codeServices = codeServices;
        }

        [Route("api/presentations"), HttpGet]
        public async Task<IEnumerable<Presentation>> GetItems()
        {
            return await _codeServices.GetItems();
        }

        [Route("api/presentations/add"), HttpPost]
        public async Task<bool> AddItem([FromBody] Presentation item)
        {
            return await _codeServices.AddItem(item);
        }

        [Route("api/presentations/remove/{propertyName}/{id}"), HttpDelete]
        public async Task<bool> RemoveItem(string propertyName, string id)
        {
            return await _codeServices.RemoveItem(propertyName, id);
        }
    }
}
