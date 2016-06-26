using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Trainer.Domain;

namespace Code.Service
{
    public class ComponentsController: ApiController
    {
        private readonly ICodeServices _codeServices;  
        
        //todo: need to propery configure IoC resolver
        public ComponentsController()
        {
            _codeServices = (CodeService.Container.Resolve<ICodeServices>());
        }      

        public ComponentsController(ICodeServices codeServices)
        {
            _codeServices = codeServices;
        }

        [Route("api/components"), HttpGet]
        public async Task<IEnumerable<Component>> GetItems()
        {
            return await _codeServices.GetItems();
        }

        //[Route("api/components/additems"), HttpPost]
        //public async Task<bool> AddItems([FromBody] IEnumerable<Component> items)
        //{
        //    return await _codeServices.AddItems(items);
        //}

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
