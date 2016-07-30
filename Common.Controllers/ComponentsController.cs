using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Trainer.Domain;

namespace Common.Controllers
{
    public interface ICodeServices
    {
        Task<IEnumerable<Component>> GetItems();
        Task<bool> AddItems(IEnumerable<Component> items);
        Task<bool> RemoveItem(string id);
    }

    public class ComponentsController: ApiController
    {
        private readonly ICodeServices _codeServices;  
        
        public ComponentsController(ICodeServices codeServices)
        {
            _codeServices = codeServices;
        }

        [Route("api/components"), HttpGet]
        public async Task<IEnumerable<Component>> GetItems()
        {
            return await _codeServices.GetItems();
        }

        [Route("api/components/add"), HttpPost]
        public async Task<bool> AddItem([FromBody] Components item)
        {
            return await _codeServices.AddItems(item.Component);
        }

        [Route("api/components/remove/{id}"), HttpDelete]
        public async Task<bool> RemoveItem(string id)
        {
            return await _codeServices.RemoveItem(id);
        }
    }
}
